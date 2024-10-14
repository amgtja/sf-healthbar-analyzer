//using OpenCvSharp;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace HealthBar {
//    internal class hakaba {
//        public double CaliculateHPBar(Mat currentFrameMat) {
//            if (!isSettingComplete || currentFrameMat == null) { return -1; }
//            Mat currentRoi = new Mat(currentFrameMat, new OpenCvSharp.Rect(HPBarArea.X, HPBarArea.Y, HPBarArea.Width, HPBarArea.Height));
//            Mat graycurrentRoi = new Mat();
//            Cv2.CvtColor(currentRoi, graycurrentRoi, ColorConversionCodes.BGR2GRAY);

//            //前フレームのチェック
//            if (previousFrameMat == null) {
//                previousFrameMat = graycurrentRoi.Clone();
//                return 100.0;
//            }
//            //差分計算
//            Mat diff = new Mat();
//            Cv2.Absdiff(previousFrameMat, graycurrentRoi, diff);
//            //差分のうち、一定以上のピクセル値を体力減少としてカウント(変数:thValue)
//            Mat mask = new Mat();
//            Cv2.Threshold(diff, mask, thValue, 255, ThresholdTypes.Binary);

//            //差分のピクセル数をカウント
//            int changingPixels = Cv2.CountNonZero(mask);
//            int totalPixels = HPBarArea.Width * HPBarArea.Height;

//            //現在の体力を返す
//            double healthPercent = 100 * (1.0 - (double)changingPixels / totalPixels);
//            healthPercents.Add(healthPercent);

//            return healthPercent;
//        }
//        public void DrawHealthBarGraph(int currentFrameIndex) {
//            if (healthPercents.Count == 0 || currentFrameIndex >= healthPercents.Count) return;
//            Bitmap graphBitmap = new Bitmap(pictureBoxHP.Width, pictureBoxHP.Height);
//            using (Graphics g = Graphics.FromImage(graphBitmap)) {
//                g.Clear(Color.White);
//                int barWidth = pictureBoxHP.Width / healthPercents.Count;
//                int maxHeight = pictureBoxHP.Height;

//                for (int i = 0; i < healthPercents.Count; i++) {
//                    double percent = healthPercents[i];
//                    int barHeight = (int)(maxHeight * (percent / 100));
//                    //ハイライト
//                    Brush brush = i == currentFrameIndex ? Brushes.Red : Brushes.Blue;
//                    g.FillRectangle(brush, i * barWidth, maxHeight - barHeight, barWidth - 2, barHeight);
//                }
//            }
//            pictureBoxHP.Image = graphBitmap;
//        }
//        public void ExportHPDataToPicture() {
//            if (!isSettingComplete) { return; }
//            for (int i = 0; i <= trackBarFrame.Maximum; i++) {
//                Bitmap frame = videoL.GetFrameAt(i);
//                Mat frameMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(frame);
//                double healthPercentage = CaliculateHPBar(frameMat);
//            }
//        }

//        public void ExportHPDataCSV(string filePath) {
//            if (!isSettingComplete) { return; }
//            using (StreamWriter writer = new StreamWriter(filePath)) {
//                writer.WriteLine("Frame,HealthPercentage");
//                for (int i = 0; i <= trackBarFrame.Maximum; i++) {
//                    Bitmap frame = videoL.GetFrameAt(i);
//                    Mat frameMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(frame);
//                    double healthPercentage = CaliculateHPBar(frameMat);
//                    writer.WriteLine($"{i},{healthPercentage}");
//                }
//            }
//            MessageBox.Show("CSV出力完了");
//        }
//public void AnalyzeB_Click(object sender, EventArgs e) {
//    SaveFileDialog saveFileDialog = new SaveFileDialog();
//    saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
//    saveFileDialog.Title = "体力ゲージデータを保存";
//    //ファイル選択したとき
//    if (saveFileDialog.ShowDialog() == DialogResult.OK) {
//        string filePath = saveFileDialog.FileName;
//        ExportHPDataCSV(filePath);
//        //ExportHPDataToPicture();
//    }
//}
//    }
//}
// 輝度値を表示（例えば、メッセージボックスに表示）
//StringBuilder sb = new StringBuilder();
//sb.AppendLine($"Y座標: {e.Y} の輝度値:");
//foreach (var brightness in brightnessValues) {
//    sb.Append($"{brightness} ");
//}
//MessageBox.Show(sb.ToString(), "Brightness Values", MessageBoxButtons.OK, MessageBoxIcon.Information);