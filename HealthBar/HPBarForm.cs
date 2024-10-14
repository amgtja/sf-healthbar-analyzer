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

namespace HealthBar {
    public partial class HPBarForm : Form {

        public List<System.Drawing.Point> points = new List<System.Drawing.Point>();
        public List<double> healthPercents = new List<double>();
        public List<System.Drawing.Point> boundaryPoints = new List<System.Drawing.Point>();
        public List<int> boundaries = new List<int>();
        public List<int> gradients = new List<int>();
        public VideoLoader videoL;
        public Analizer analizer;
        public bool mouseDrug = false;
        public Rectangle HPBarArea;
        public Mat previousFrameMat;
        public int thValue = 30;
        public string selectedF = null;
        public int selectedY = 0;
        public HPBarForm() {
            InitializeComponent();
            videoL = new VideoLoader();
            analizer = new Analizer();

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
            pictureBoxFrame.MouseClick += PictureBoxFrame_MouseClick;
            pictureBoxBW.MouseClick += PictureBoxBW_MouseClick;

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

        public void PictureBoxFrame_MouseClick(object sender, MouseEventArgs e) {
            //クリックされたY座標の取得
            selectedY = e.Y;
            BrightText.Text = selectedY.ToString();

            //RGB値を出してみる
            var rgbValues = GetRGB(trackBarFrame.Value, selectedY);
            DrawChartRGB(rgbValues);

            //輝度を出してみる
            List<byte> brightnessValues = GetBright(trackBarFrame.Value, selectedY);
            DrawChart(brightnessValues);

            //隣のピクセルとの差分を見てみる
            List<int> gradients = Gradient1(trackBarFrame.Value, selectedY);
            DrawChartGradient(gradients);

            //境界線を探す
            List<int> boundaries = FindBoundary(gradients);
            string boundariesString = string.Join(", ", boundaries);

            //境界点を描写したい
            boundaryPoints.Clear();
            foreach (int x in boundaries) {
                boundaryPoints.Add(new System.Drawing.Point(x, selectedY));
            }
            pictureBoxFrame.Invalidate();
            //MessageBox.Show(boundariesString, "Boundaries", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void PictureBoxFrame_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            Brush brush = Brushes.Red; // 点の色を指定
            int pointSize = 5; // 点のサイズ

            // 境界のポイントを描画
            foreach (System.Drawing.Point point in boundaryPoints) {
                g.FillEllipse(brush, point.X - pointSize / 2, point.Y - pointSize / 2, pointSize, pointSize);
            }
        }

        public void PictureBoxBW_MouseClick(object sender, MouseEventArgs e) {
            selectedY = e.Y;

            // クリックされたY座標の輝度値を全て取得
            List<byte> brightnessValues = GetBright(trackBarFrame.Value, selectedY);
            DrawChart(brightnessValues);

        }
        public void DrawChart(List<byte> data) {
            //初期化
            chartDataGray.Series.Clear();
            chartDataGray.ChartAreas.Clear();
            //新しいエリアとシリーズの確保
            ChartArea chartArea = new ChartArea("BrightnessArea");
            chartDataGray.ChartAreas.Add(chartArea);
            Series series = new Series("Brightness") {
                ChartType = SeriesChartType.Column
            };
            chartDataGray.Series.Add(series);
            //凡例なし
            series.IsVisibleInLegend = false;
            //輝度データ追加
            for (int x = 0; x < data.Count; x++) {
                series.Points.AddXY(x, data[x]);
            }
            chartDataGray.ChartAreas["BrightnessArea"].AxisX.Title = "X座標";
            chartDataGray.ChartAreas["BrightnessArea"].RecalculateAxesScale();
        }
        public void DrawChartGradient(List<int> data) {
            //初期化
            chartG.Series.Clear();
            chartG.ChartAreas.Clear();
            //新しいエリアとシリーズの確保
            ChartArea chartArea = new ChartArea("GradientsArea");
            chartG.ChartAreas.Add(chartArea);
            Series series = new Series("Gradients") {
                ChartType = SeriesChartType.Column
            };
            chartG.Series.Add(series);
            //凡例なし
            series.IsVisibleInLegend = false;
            //輝度データ追加
            for (int x = 0; x < data.Count; x++) {
                series.Points.AddXY(x, data[x]);
            }
            chartG.ChartAreas["GradientsArea"].AxisX.Title = "X座標";
            chartG.ChartAreas["GradientsArea"].RecalculateAxesScale();
        }
        public void DrawChartRGB((List<byte> R, List<byte> G, List<byte> B) rgbValues) {
            chartData.Series.Clear();
            chartData.ChartAreas.Clear();
            ChartArea chartArea = new ChartArea("RGBArea");
            chartData.ChartAreas.Add(chartArea);
            Series seriesR = new Series("Red") {
                ChartType = SeriesChartType.Line,
                Color = Color.Red
            };
            Series seriesG = new Series("Green") {
                ChartType = SeriesChartType.Line,
                Color = Color.Green
            };
            Series seriesB = new Series("Blue") {
                ChartType = SeriesChartType.Line,
                Color = Color.Blue
            };
            chartData.Series.Add(seriesR);
            chartData.Series.Add(seriesG);
            chartData.Series.Add(seriesB);
            //凡例なし
            seriesR.IsVisibleInLegend = false;
            seriesG.IsVisibleInLegend = false;
            seriesB.IsVisibleInLegend = false;

            for (int x = 0; x < rgbValues.R.Count; x++) {
                seriesR.Points.AddXY(x, rgbValues.R[x]);
                seriesG.Points.AddXY(x, rgbValues.G[x]);
                seriesB.Points.AddXY(x, rgbValues.B[x]);
            }
            chartData.ChartAreas["RGBArea"].AxisX.Title = "X座標";
            chartData.ChartAreas["RGBArea"].RecalculateAxesScale();

        }
        public (List<byte> R, List<byte> G, List<byte> B) GetRGB(int currentFrame, int y) {
            List<byte> rValues = new List<byte>();
            List<byte> gValues = new List<byte>();
            List<byte> bValues = new List<byte>();

            Bitmap frameBitmap = videoL.GetFrameAt(currentFrame);
            Mat frame = OpenCvSharp.Extensions.BitmapConverter.ToMat(frameBitmap);

            for (int x = 0; x < frame.Width; x++) {
                Vec3b color = frame.At<Vec3b>(y, x); // OpenCVのVec3b型でRGB値を取得
                bValues.Add(color.Item0); // B成分
                gValues.Add(color.Item1); // G成分
                rValues.Add(color.Item2); // R成分
            }

            return (rValues, gValues, bValues);
        }

        public List<int> Gradient1(int currentFrame, int y) {
            List<int> gradients = new List<int>();

            Bitmap frameBitmap = videoL.GetFrameAt(currentFrame);
            Mat frame = OpenCvSharp.Extensions.BitmapConverter.ToMat(frameBitmap);

            // RGB値の総和を計算し、輝度の代わり
            List<int> intensityValues = new List<int>();

            for (int x = 0; x < frame.Width; x++) {
                Vec3b color = frame.At<Vec3b>(y, x);
                int intensity = color.Item0 + color.Item1 + color.Item2; // R + G + Bの総和を計算
                intensityValues.Add(intensity);
            }

            // 勾配（一次微分）の計算
            for (int i = 1; i < intensityValues.Count; i++) {
                int gradient = Math.Abs(intensityValues[i] - intensityValues[i - 1]);
                gradients.Add(gradient);
            }

            //gradient[X座標]で0に1のX座標が入っている、RGBの総和を出す
            return gradients;

        }

        //境界点を見つけるメソッド
        public List<int> FindBoundary(List<int> gradient) {
            List<int> boundaries = new List<int>();
            int temp = 0;
            bool mode = true; // 境界内を見つけるモード
            int threshold = 100; // 閾値を大きめに設定
            int continuousPixels = 60; // 連続する「ほぼ0」領域の最小長さ
            int count = 0;
            for (int i = 1; i < gradient.Count; i++) {
                if (count == 4) { break; }
                int diff = Math.Abs(gradient[i] - gradient[i - 1]);
                if (mode) {
                    if (diff < threshold) {
                        temp++;
                    } else {
                        temp = 0;
                    }

                    if (temp >= continuousPixels) {
                        boundaries.Add(i - continuousPixels);
                        mode = false;
                        temp = 0;
                        count++;
                    }
                } else {
                    if (diff > threshold) {
                        boundaries.Add(i);
                        mode = true;
                        count++;
                    }
                }
            }

            return boundaries;
        }

        public int AnalizeBoundary(List<int> boundaries, List<int> gradient) {
            int totalFrame = videoL.capture.FrameCount;
            int threshold = 100;
            int boundaryMax1P = boundaries[0];
            int boundaryMin1P = boundaries[1];
            int boundaryTemp1P = boundaryMax1P;
            int maxHP = boundaryMax1P;
            for (int j = (boundaryMin1P - 1); boundaryMax1P <= j; j--) {
                if (gradient[j] > threshold) {
                    boundaryTemp1P = j;
                    break;
                }
                //そのフレームにおける体力の境界が出る
                maxHP = boundaryTemp1P;
            }
            return maxHP;
        }


        private void FileDisplay_TextChanged(object sender, EventArgs e) {

        }

        public void TrackBarFrame_Scroll(object sender, EventArgs e) {
            pictureBoxFrame.Image = videoL.GetFrameAt(trackBarFrame.Value);
            pictureBoxBW.Image = videoL.ToBW(trackBarFrame.Value);
        }

        public void PictureBoxFrame_MouseDown(object sender, MouseEventArgs e) {
            mouseDrug = true;

        }

        private void TextBox1_TextChanged(object sender, EventArgs e) {

        }

        private void AnalyzeB_Click(object sender, EventArgs e) {
            //int max = AnalizeBoundary(boundaries,gradients);
            //MessageBox.Show(max.ToString());
            string boundariesString = string.Join(", ", boundaries);
            MessageBox.Show(boundariesString);
        }
    }
}
