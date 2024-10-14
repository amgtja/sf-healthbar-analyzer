using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using OpenCvSharp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HealthBar {
    public partial class HPBarForm : Form {
        public List<double> healthPercents = new List<double>();
        public List<System.Drawing.Point> boundaryPoints = new List<System.Drawing.Point>();
        public List<int> boundaries = new List<int>();
        public List<int> gradients = new List<int>();
        public List<byte> brightnessValues = new List<byte>();
        public (List<byte>, List<byte>, List<byte>) rgbValues = (new List<byte>(), new List<byte>(), new List<byte>());
        VideoLoader videoL = new VideoLoader();
        public Charts charts;
        public Caliculate caliculate;
        public Boundary boundary;

        public string filePath = null;
        public int selectedY = 0;
        public HPBarForm() {
            InitializeComponent();
            charts = new Charts(this);
            boundary = new Boundary(this);
            caliculate = new Caliculate(this);


            //ファイルパス表示or非表示
            FileDisplay.Visible = false;

            //Paintイベント
            pictureBoxFrame.Paint += PictureBoxFrame_Paint;
        }

        public void HPBar_Load(object sender, EventArgs e) {
            trackBarFrame.Minimum = 0;
            trackBarFrame.Scroll += TrackBarFrame_Scroll;
        }

        public void FileSelectB_Click(object sender, EventArgs e) {
            //selectedFにファイルパス入れる
            //0フレーム目の取得、pictureBoxFrameにBitmap形式のframeを代入して表示させる

            //FileSelectorクラスのインスタンス作成、動画ファイル選択
            FileSelector fileS = new FileSelector();
            filePath = fileS.SelectVideoFile();

            //テキストボックスにファイルパスを表示
            FileDisplay.Text = filePath;

            //動画ファイルを開く
            if (!string.IsNullOrEmpty(filePath) && videoL.LoadVideo(filePath)) {

                //最初のフレームを取得
                Bitmap frame = videoL.GetFrameAt(0);
                if (frame != null) {
                    pictureBoxFrame.Image = frame;
                    //trackBarFrameのMax設定
                    trackBarFrame.Maximum = videoL.TotalFrames - 1;
                } else {
                    MessageBox.Show("フレーム取得失敗", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("動画の読み込みに失敗しました", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            caliculate.LoadVideo(filePath);

        }

        public void ConfigB_Click(object sender, EventArgs e) {
            pictureBoxFrame.MouseClick += PictureBoxFrame_MouseClick;
            pictureBoxBW.MouseClick += PictureBoxBW_MouseClick;

        }
        public void PictureBoxFrame_MouseClick(object sender, MouseEventArgs e) {
            //クリックされたY座標の取得
            selectedY = e.Y;
            BrightText.Text = selectedY.ToString();

            //RGB値を出してみる
            rgbValues = caliculate.GetRGB(trackBarFrame.Value, selectedY);
            charts.DrawChartRGB(rgbValues);

            //輝度を出してみる
            brightnessValues = caliculate.GetBright(trackBarFrame.Value, selectedY);
            charts.DrawChart(brightnessValues);

            //隣のピクセルとの差分を見てみる
            gradients = caliculate.Gradient1(trackBarFrame.Value, selectedY);
            charts.DrawChartGradient(gradients);

            //境界線を探す
            boundaries = boundary.FindBoundary(gradients);
            //string boundariesString = string.Join(", ", boundaries);

            //境界点を描写したい
            boundaryPoints.Clear();
            foreach (int x in boundaries) {
                boundaryPoints.Add(new System.Drawing.Point(x, selectedY));
            }
            pictureBoxFrame.Invalidate();
            //MessageBox.Show(boundariesString, "Boundaries", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        public void PictureBoxFrame_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            Brush brush = Brushes.Red; // 点の色を指定
            int pointSize = 5; // 点のサイズ

            // 境界のポイントを描画
            foreach (System.Drawing.Point point in boundaryPoints) {
                g.FillEllipse(brush, point.X - pointSize / 2, point.Y - pointSize / 2, pointSize, pointSize);
            }
            DrawHealthPercentageBar(e.Graphics);
        }
        private void DrawHealthPercentageBar(Graphics g) {
            if (healthPercents.Count == 0) {
                return; // 解析結果がない場合は描画しない
            }

            // 描画のためのバーの高さと位置を設定
            int barHeight = 20;
            int barY = pictureBoxFrame.Height - barHeight - 20; 

            // 横棒の背景を描画
            g.FillRectangle(Brushes.Gray, boundary.maxHPBoundary, barY, boundary.minHPBoundary-boundary.maxHPBoundary, barHeight);

            // 現在のフレームの体力割合に応じたバーを描画
            int currentFrameIndex = trackBarFrame.Value;
            if (currentFrameIndex < healthPercents.Count) {
                double hpPercent = healthPercents[currentFrameIndex];
                Console.WriteLine($"Frame: {currentFrameIndex}, HP Percent: {hpPercent}");

                int maxWidth = boundary.minHPBoundary - boundary.maxHPBoundary;
                int barWidth = (int)(maxWidth * hpPercent / 100.0);

                // 体力割合を示すバーを描画（緑色）
                g.FillRectangle(Brushes.Green, boundary.maxHPBoundary, barY,barWidth,barHeight);

                // 体力%をテキストで表示
                string percentText = $"{hpPercent:F1}%";
                g.DrawString(percentText, this.Font, Brushes.White, barWidth + 5, barY - 15);
            }
        }
        public void UpdateHPDisplay() {
            pictureBoxFrame.Invalidate();
        }

        public void PictureBoxBW_MouseClick(object sender, MouseEventArgs e) {
            selectedY = e.Y;

            // クリックされたY座標の輝度値を全て取得
            List<byte> brightnessValues = caliculate.GetBright(trackBarFrame.Value, selectedY);
            charts.DrawChart(brightnessValues);

        }


        public void TrackBarFrame_Scroll(object sender, EventArgs e) {
            pictureBoxFrame.Image = videoL.GetFrameAt(trackBarFrame.Value);
            pictureBoxBW.Image = videoL.ToBW(trackBarFrame.Value);
        }


        public void AnalyzeB_Click(object sender, EventArgs e) {
            string boundariesString = string.Join(", ", boundaries);
            MessageBox.Show(boundariesString, "Boundaries", MessageBoxButtons.OK, MessageBoxIcon.Information);
            boundary.LoadVideo(filePath);
        }

        public void SetBaseBoundaryB_Click(object sender, EventArgs e) {
            boundary.SetBaseBoundaries(boundaries);
        }

        public async void CaliculateAllFramesB_Click(object sender, EventArgs e) {
            var progress = new Progress<int>(percent => this.progressBar.Value = percent);
            await boundary.CaliculateAllFrameHP(progress);
            UpdateHPDisplay();
        }

        public void SaveToCSVB_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                Filter = "CSVファイル (*.csv)|*.csv",
                Title = "保存先のファイルを指定してください"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                boundary.SaveHPPercentagesToCSV(saveFileDialog.FileName);
            }
        }
        public void pictureBoxBW_Click(object sender, EventArgs e) {

        }

        public void chartDataGray_Click(object sender, EventArgs e) {

        }

        public void PictureBoxFrame_MouseDown(object sender, MouseEventArgs e) {

        }

        public void TextBox1_TextChanged(object sender, EventArgs e) {

        }

        public void FileDisplay_TextChanged(object sender, EventArgs e) {

        }
    }
}
