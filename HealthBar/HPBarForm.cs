using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using OpenCvSharp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HealthBar {
    public partial class HPBarForm : Form {
        public List<double> healthPercents1P = new List<double>();
        public List<double> healthPercents2P = new List<double>();
        public List<string> errorList = new List<string>();
        public List<System.Drawing.Point> boundaryPoints = new List<System.Drawing.Point>();
        public List<int> boundaries = new List<int>();
        public List<int> gradients = new List<int>();
        public List<byte> brightnessValues = new List<byte>();
        public (List<byte>, List<byte>, List<byte>) rgbValues = (new List<byte>(), new List<byte>(), new List<byte>());
        public VideoLoader videoL;
        public Charts charts;
        public Caliculate caliculate;
        public Boundary boundary;
        //chartのON/OFF機能
        readonly bool chartOn = true;

        public CancellationTokenSource cancellationTokenSource;

        public string filePath = null;
        public int selectedY = 0;
        public HPBarForm() {
            InitializeComponent();
            videoL = new VideoLoader(this);
            charts = new Charts(this);
            boundary = new Boundary(this);
            caliculate = new Caliculate(this);


            //ファイルパス表示or非表示
            FileDisplay.Visible = false;
            pictureBoxBW.Visible = false;
            chartDataGray.Visible = false;
            //chartData.Visible = false;
            //chartG.Visible = false;



            //Paintイベント
            pictureBoxFrame.Paint += PictureBoxFrame_Paint;
        }

        public void HPBar_Load(object sender, EventArgs e) {
            trackBarFrame.Minimum = 0;
            trackBarFrame.Scroll += TrackBarFrame_Scroll;
            //再生ボタンbtnPlayの表示
            btnPlay.Text = "動画がロードされていません";
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
                Bitmap frame = videoL.GetFrameRead(1);
                if (frame != null) {
                    pictureBoxFrame.Image = frame;
                    //trackBarFrameのMax設定
                    trackBarFrame.Minimum = 0;
                    trackBarFrame.Maximum = videoL.TotalFrames - 1;
                    trackBarFrame.SmallChange = 1; // 矢印キーやクリックで1フレーム進む
                    trackBarFrame.LargeChange = 1; // ページアップ・ダウンで1フレーム進む
                    btnPlay.Text = "再生/停止";

                } else {
                    MessageBox.Show("フレーム取得失敗", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("動画の読み込みに失敗しました", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ConfigB_Click(object sender, EventArgs e) {
            pictureBoxFrame.MouseClick += PictureBoxFrame_MouseClick;
            pictureBoxBW.MouseClick += PictureBoxBW_MouseClick;

        }
        public void PictureBoxFrame_MouseClick(object sender, MouseEventArgs e) {
            //クリックされたY座標の取得
            selectedY = e.Y;
            BrightText.Text = selectedY.ToString();

            //境界線を探す
            gradients = caliculate.Gradient1(trackBarFrame.Value, selectedY);
            boundaries = boundary.FindBoundary(gradients);
            //string boundariesString = string.Join(", ", boundaries);

            //境界点を描写したい
            boundaryPoints.Clear();
            foreach (int x in boundaries) {
                boundaryPoints.Add(new System.Drawing.Point(x, selectedY));
            }
            pictureBoxFrame.Refresh();
            //MessageBox.Show(boundariesString, "Boundaries", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (chartOn) {
                //RGB値を出してみる
                rgbValues = caliculate.GetRGB(trackBarFrame.Value, selectedY);
                charts.DrawChartRGB(rgbValues);

                //輝度を出してみる
                //brightnessValues = caliculate.GetBright(trackBarFrame.Value, selectedY);
                //charts.DrawChart(brightnessValues);

                //隣のピクセルとの差分を見てみる

                charts.DrawChartGradient(gradients);
            }
        }
        public void PictureBoxFrame_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            Brush brush = Brushes.Red; // 点の色を指定
            int pointSize = 8; // 点のサイズ

            // 境界のポイントを描画
            foreach (System.Drawing.Point point in boundaryPoints) {
                g.FillEllipse(brush, point.X - pointSize / 2, point.Y - pointSize / 2, pointSize, pointSize);
            }
            if (boundaryPoints.Count == 4) {
                Pen redPen = new Pen(Color.Red, 2);
                // 1Pの線分
                g.DrawLine(redPen, boundaryPoints[0].X, boundaryPoints[0].Y, boundaryPoints[1].X, boundaryPoints[1].Y);

                // 2Pの線分
                g.DrawLine(redPen, boundaryPoints[2].X, boundaryPoints[2].Y, boundaryPoints[3].X, boundaryPoints[3].Y);
                redPen.Dispose();
            }

            DrawHealthPercentageBar(e.Graphics);
        }
        private void DrawHealthPercentageBar(Graphics g) {
            if (healthPercents1P.Count == 0) {
                return; // 解析結果がない場合は描画しない
            }

            // 描画のためのバーの高さと位置を設定
            int barHeight = 20;
            int barY = pictureBoxFrame.Height - 500;

            // 横棒の背景を描画

            // 現在のフレームの体力割合に応じたバーを描画
            int currentFrameIndex = trackBarFrame.Value;
            if (currentFrameIndex < healthPercents1P.Count) {
                double hpPercent1P = healthPercents1P[currentFrameIndex];
                double hpPercent2P = healthPercents2P[currentFrameIndex];
                Console.WriteLine($"Frame: {currentFrameIndex}, HP Percent: {hpPercent1P},{hpPercent2P}");

                int maxWidth1P = boundary.minHPBoundary1P - boundary.maxHPBoundary1P;
                int barWidth1P = (int)(maxWidth1P * (1 - hpPercent1P / 100.0));
                int maxWidth2P = boundary.maxHPBoundary2P - boundary.minHPBoundary2P;
                int barWidth2P = (int)(maxWidth2P * (1 - hpPercent2P / 100.0));

                g.FillRectangle(Brushes.Green, boundary.maxHPBoundary1P, barY, (boundary.minHPBoundary1P - boundary.maxHPBoundary1P), barHeight);
                //2P

                g.FillRectangle(Brushes.Green, boundary.minHPBoundary2P, barY, (boundary.maxHPBoundary2P - boundary.minHPBoundary2P), barHeight);
                // 体力割合を示すバーを描画（灰色で削るスタイル）
                g.FillRectangle(Brushes.Gray, boundary.maxHPBoundary1P, barY, barWidth1P, barHeight);
                //2P
                g.FillRectangle(Brushes.Gray, boundary.minHPBoundary2P, barY, barWidth2P, barHeight);
                // 体力%をテキストで表示
                string percentText = $"{hpPercent1P:F1}%,+{","}+{hpPercent2P:F1}%";
                HealthText.Text = percentText;
                textBoxError.Text = errorList[(currentFrameIndex)];
            }

        }
        public void UpdateHPDisplay() {
            pictureBoxFrame.Invalidate();
        }

        public void PictureBoxBW_MouseClick(object sender, MouseEventArgs e) {
            selectedY = e.Y;

            //// クリックされたY座標の輝度値を全て取得
            //List<byte> brightnessValues = caliculate.GetBright(trackBarFrame.Value, selectedY);
            //charts.DrawChart(brightnessValues);

        }


        public void TrackBarFrame_Scroll(object sender, EventArgs e) {
            pictureBoxFrame.Image = videoL.GetFrameRead(trackBarFrame.Value);
            FrameBox.Text = (videoL.currentframe.ToString());
        }


        public void AnalyzeB_Click(object sender, EventArgs e) {
            string boundariesString = string.Join(", ", boundaries);
            MessageBox.Show(boundariesString, "Boundaries", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void SetBaseBoundaryB_Click(object sender, EventArgs e) {
            boundary.SetBaseBoundaries(boundaries);
        }

        public async void CaliculateAllFramesB_Click(object sender, EventArgs e) {
            var progress = new Progress<int>(percent => this.progressBar.Value = percent);
            await boundary.CaliculateAllFrameHP(progress);
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

        private void pictureBoxFrame_Click(object sender, EventArgs e) {

        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void btnPlay_Click(object sender, EventArgs e) {
            if (timerFramePlay.Enabled) {
                timerFramePlay.Enabled = false;
            } else {
                timerFramePlay.Enabled = true;
            }
        }

        private void timerFramePlay_Tick(object sender, EventArgs e) {
            if ((trackBarFrame.Value < videoL.TotalFrames - 1)) {
                pictureBoxFrame.Image.Dispose();
                Bitmap frame = videoL.GetFrameRead(trackBarFrame.Value);
                pictureBoxFrame.Image = frame;
                trackBarFrame.Value++;
            } else {
                timerFramePlay.Enabled = false;
            }
        }

        private void trackBarFrame_ValueChanged(object sender, EventArgs e) {
            FrameBox.Text = (videoL.currentframe.ToString());
        }
    }
}
