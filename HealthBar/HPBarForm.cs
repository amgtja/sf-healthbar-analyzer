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
using OpenCvSharp;

namespace HealthBar {
    public partial class HPBarForm : Form {

        public List<System.Drawing.Point> points = new List<System.Drawing.Point>();
        public List<double> healthPercents = new List<double>();
        bool isSettingComplete = false;
        public VideoLoader videoL;
        public bool mouseDrug = false;
        public Rectangle HPBarArea;
        public Mat previousFrameMat;
        public int thValue = 30;
        public string selectedF = null;
        public int selectedY = 0;
        public HPBarForm() {
            InitializeComponent();
            videoL = new VideoLoader();

            //ファイルパス表示or非表示
            FileDisplay.Visible = false;
        }

        public void HPBar_Load(object sender, EventArgs e) {
            trackBarFrame.Minimum = 0;
            trackBarFrame.Scroll += trackBarFrame_Scroll;
            if (videoL.TotalFrames > 0) {
                DrawHealthBarGraph(0);
            }
        }

        public void FileSelectB_Click(object sender, EventArgs e) {
            //selectedFにファイルパス入れる
            //0フレーム目の取得、pictureBoxFrameにBitmap形式のframeを代入して表示させる

            //FileSelectorクラスのインスタンス作成、動画ファイル選択
            FileSelector fileS = new FileSelector();
            selectedF = fileS.SelectVideoFile();

            //テキストボックスにファイルパスを表示
            FileDisplay.Text = selectedF;

            //動画ファイルを開く
            if (!string.IsNullOrEmpty(selectedF) && videoL.LoadVideo(selectedF)) {

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

        }

        public void ConfigB_Click(object sender, EventArgs e) {
            pictureBoxFrame.MouseClick += pictureBoxFrame_MouseClick;
            //points.Clear();

        }
        public List<byte> GetBright(int currentFrame, int y) {
            List<byte> brightValue = new List<byte>();
            int width = videoL.GetFrameAt(currentFrame).Width;
            Mat frame = OpenCvSharp.Extensions.BitmapConverter.ToMat(videoL.GetFrameAt(currentFrame));
            for (int x = 0; x < width; x++) {
                byte brightness = frame.At<byte>(y, x);
                brightValue.Add(brightness);
            }
            return brightValue;
        }

        public void pictureBoxFrame_MouseClick(object sender, MouseEventArgs e) {
            //クリックされた座標を取得したい
            selectedY = e.Y;
            BrightText.Text = selectedY.ToString();
            // クリックされたY座標の輝度値を全て取得
            List<byte> brightnessValues = GetBright(trackBarFrame.Value,selectedY);

            // 輝度値を表示（例えば、メッセージボックスに表示）
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Y座標: {e.Y} の輝度値:");
            foreach (var brightness in brightnessValues) {
                sb.Append($"{brightness} ");
            }
            MessageBox.Show(sb.ToString(), "Brightness Values", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //if (points.Count < 4) {
            //    points.Add(e.Location);
            //    if (points.Count == 4) {
            //        int xMin = Math.Min(Math.Min(points[0].X, points[1].X), Math.Min(points[2].X, points[3].X));
            //        int xMax = Math.Max(Math.Max(points[0].X, points[1].X), Math.Max(points[2].X, points[3].X));
            //        int yMin = Math.Min(Math.Min(points[0].Y, points[1].Y), Math.Min(points[2].Y, points[3].Y));
            //        int yMax = Math.Max(Math.Max(points[0].Y, points[1].Y), Math.Min(points[2].Y, points[3].Y));
            //        HPBarArea = new Rectangle(xMin, yMin, xMax - xMin, yMax - yMin);
            //        isSettingComplete = true;
            //        MessageBox.Show("体力ゲージ範囲設定完了");
            //    }
            //}
        }
        public double CaliculateHPBar(Mat currentFrameMat) {
            if (!isSettingComplete || currentFrameMat == null) { return -1; }
            Mat currentRoi = new Mat(currentFrameMat, new OpenCvSharp.Rect(HPBarArea.X, HPBarArea.Y, HPBarArea.Width, HPBarArea.Height));
            Mat graycurrentRoi = new Mat();
            Cv2.CvtColor(currentRoi, graycurrentRoi, ColorConversionCodes.BGR2GRAY);

            //前フレームのチェック
            if (previousFrameMat == null) {
                previousFrameMat = graycurrentRoi.Clone();
                return 100.0;
            }
            //差分計算
            Mat diff = new Mat();
            Cv2.Absdiff(previousFrameMat, graycurrentRoi, diff);
            //差分のうち、一定以上のピクセル値を体力減少としてカウント(変数:thValue)
            Mat mask = new Mat();
            Cv2.Threshold(diff, mask, thValue, 255, ThresholdTypes.Binary);

            //差分のピクセル数をカウント
            int changingPixels = Cv2.CountNonZero(mask);
            int totalPixels = HPBarArea.Width * HPBarArea.Height;

            //現在の体力を返す
            double healthPercent = 100 * (1.0 - (double)changingPixels / totalPixels);
            healthPercents.Add(healthPercent);

            return healthPercent;
        }
        public void DrawHealthBarGraph(int currentFrameIndex) {
            if (healthPercents.Count == 0 || currentFrameIndex >= healthPercents.Count) return;
            Bitmap graphBitmap = new Bitmap(pictureBoxHP.Width, pictureBoxHP.Height);
            using (Graphics g = Graphics.FromImage(graphBitmap)) {
                g.Clear(Color.White);
                int barWidth = pictureBoxHP.Width / healthPercents.Count;
                int maxHeight = pictureBoxHP.Height;

                for (int i = 0; i < healthPercents.Count; i++) {
                    double percent = healthPercents[i];
                    int barHeight = (int)(maxHeight * (percent / 100));
                    //ハイライト
                    Brush brush = i == currentFrameIndex ? Brushes.Red : Brushes.Blue;
                    g.FillRectangle(brush, i * barWidth, maxHeight - barHeight, barWidth - 2, barHeight);
                }
            }
            pictureBoxHP.Image = graphBitmap;
        }
        public void ExportHPDataToPicture() {
            if (!isSettingComplete) { return; }
            for (int i = 0; i <= trackBarFrame.Maximum; i++) {
                Bitmap frame = videoL.GetFrameAt(i);
                Mat frameMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(frame);
                double healthPercentage = CaliculateHPBar(frameMat);
            }
        }

        public void ExportHPDataCSV(string filePath) {
            if (!isSettingComplete) { return; }
            using (StreamWriter writer = new StreamWriter(filePath)) {
                writer.WriteLine("Frame,HealthPercentage");
                for (int i = 0; i <= trackBarFrame.Maximum; i++) {
                    Bitmap frame = videoL.GetFrameAt(i);
                    Mat frameMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(frame);
                    double healthPercentage = CaliculateHPBar(frameMat);
                    writer.WriteLine($"{i},{healthPercentage}");
                }
            }
            MessageBox.Show("CSV出力完了");
        }

        private void FileDisplay_TextChanged(object sender, EventArgs e) {

        }

        public void trackBarFrame_Scroll(object sender, EventArgs e) {
            pictureBoxFrame.Image = videoL.GetFrameAt(trackBarFrame.Value);
            pictureBoxBW.Image = videoL.ToBW(trackBarFrame.Value);
        }

        public void pictureBoxFrame_MouseDown(object sender, MouseEventArgs e) {
            mouseDrug = true;

        }

        private void textBox1_TextChanged(object sender, EventArgs e) {

        }

        public void AnalyzeB_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            saveFileDialog.Title = "体力ゲージデータを保存";
            //ファイル選択したとき
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                string filePath = saveFileDialog.FileName;
                ExportHPDataCSV(filePath);
                //ExportHPDataToPicture();
            }
        }
    }
}
