using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HealthBar {
    public partial class HPBarForm : Form {
        private List<double> healthPercents1P = new List<double>();
        private List<double> healthPercents2P = new List<double>();
        private List<string> errorList = new List<string>();
        private AnalysisResult lastAnalysisResult;
        private List<System.Drawing.Point> boundaryPoints = new List<System.Drawing.Point>();
        private List<int> boundaries = new List<int>();
        private VideoLoader videoL;
        private Calculate calculate;
        private Boundary boundary;
        private bool SF5 = false;
        private int SF5selectY = 62;
        private bool manualSelect = false;

        private string filePath = null;
        private int selectedY = 0;
        private int selectedYfix = 0;
        public HPBarForm() {
            InitializeComponent();
            videoL = new VideoLoader(() => pictureBoxFrame.Width);
            boundary = new Boundary(videoL);
            boundary.OnMessage += msg => MessageBox.Show(msg, "HealthBar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            calculate = new Calculate(videoL);




            //Paintイベント
            pictureBoxFrame.Paint += PictureBoxFrame_Paint;
        }

        public void HPBar_Load(object sender, EventArgs e) {
            trackBarFrame.Minimum = 0;
            trackBarFrame.Scroll += TrackBarFrame_Scroll;
            //再生ボタンbtnPlayの表示
            btnPlay.Text = "動画がロードされていません";
            listBox.Items.Clear();
        }


        public void FileSelectB_Click(object sender, EventArgs e) {
            //selectedFにファイルパス入れる
            //0フレーム目の取得、pictureBoxFrameにBitmap形式のframeを代入して表示させる

            //FileSelectorクラスのインスタンス作成、動画ファイル選択
            FileSelector fileS = new FileSelector();
            filePath = fileS.SelectVideoFile();



            //動画ファイルを開く
            if (!string.IsNullOrEmpty(filePath) && videoL.LoadVideo(filePath)) {
                //最初のフレームを取得
                Bitmap frame = videoL.GetFrameRead(0);
                FrameBox.Text = ($"Frame:0");
                if (frame != null) {
                    SetDisplayFrame(frame);
                    //trackBarFrameのMax設定
                    trackBarFrame.Minimum = 0;
                    trackBarFrame.Maximum = videoL.TotalFrames - 1;
                    trackBarFrame.SmallChange = 1; // 矢印キーやクリックで1フレーム進む
                    trackBarFrame.LargeChange = 1; // ページアップ・ダウンで1フレーム進む
                    btnPlay.Text = "再生/停止";
                    //テキストボックスにファイルパスを表示
                    listBox.Items.Add($"読み込まれたファイルパス:{filePath}");
                    listBox.Items.Add($"次に解析対象を指定してください。指定なければSF6用の解析です。");
                } else {
                    MessageBox.Show("フレーム取得失敗", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("動画の読み込みに失敗しました", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ConfigB_Click(object sender, EventArgs e) {
            pictureBoxFrame.MouseClick -= PictureBoxFrame_MouseClick;
            pictureBoxFrame.MouseClick += PictureBoxFrame_MouseClick;
            listBox.Items.Add($"座標指定がアクティブになりました。体力バーをクリックしてください。");
            manualSelect = false;
        }
        private void ConfigManualB_Click(object sender, EventArgs e) {
            pictureBoxFrame.MouseClick -= PictureBoxFrame_MouseClick;
            pictureBoxFrame.MouseClick += PictureBoxFrame_MouseClick;
            listBox.Items.Add($"手動で座標を４点指定してください。Y座標は初めの１点目で決まります。");
            manualSelect = true;
            boundaries.Clear();
        }
        public void PictureBoxFrame_MouseClick(object sender, MouseEventArgs e) {
            //クリックされたY座標の取得
            if (boundaries.Count == 4) boundaries.Clear();
            selectedY = e.Y;
            if (SF5) {
                selectedY = SF5selectY;
            }
            if (manualSelect) {
                boundaries.Add(e.X);
                if (boundaries.Count == 1) selectedYfix = selectedY;
                listBox.Items.Add($"Y:{selectedYfix}で固定、X:{boundaries[boundaries.Count - 1]}");
                if (boundaries.Count == 4) {
                    selectedY = selectedYfix;
                    boundaries.Sort();
                }
            } else {
                //境界線を探す
                var gradients = calculate.Gradient1(trackBarFrame.Value, selectedY);
                boundaries = boundary.FindBoundary(gradients);
                if (SF5) {
                    boundaries = boundary.FindBoundarySF5(trackBarFrame.Value, selectedY);
                }
            }
            if (boundaries.Count == 4) {
                boundaryPoints.Clear();
                foreach (int x in boundaries) {
                    boundaryPoints.Add(new System.Drawing.Point(x, selectedY));
                }
                pictureBoxFrame.Refresh();
                listBox.Items.Add($"Y:{selectedY},X:{boundaries[0]},{boundaries[1]},{boundaries[2]},{boundaries[3]}");
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
            int barY = pictureBoxFrame.Height - 300;

            // 横棒の背景を描画
            // 現在のフレームの体力割合に応じたバーを描画
            int currentFrameIndex = trackBarFrame.Value;
            if (currentFrameIndex < healthPercents1P.Count) {
                double hpPercent1P = healthPercents1P[currentFrameIndex];
                double hpPercent2P = healthPercents2P[currentFrameIndex];

                int maxWidth1P = boundary.minHPBoundary1P - boundary.maxHPBoundary1P;
                int barWidth1P = (int)(maxWidth1P * (1 - hpPercent1P / 100.0));
                int maxWidth2P = boundary.maxHPBoundary2P - boundary.minHPBoundary2P;
                int barWidth2P = (int)(maxWidth2P * (hpPercent2P / 100.0));

                g.FillRectangle(Brushes.Green, boundary.maxHPBoundary1P, barY, (boundary.minHPBoundary1P - boundary.maxHPBoundary1P), barHeight);
                //2P
                g.FillRectangle(Brushes.Gray, boundary.minHPBoundary2P, barY, (boundary.maxHPBoundary2P - boundary.minHPBoundary2P), barHeight);
                // 体力割合を示すバーを描画
                g.FillRectangle(Brushes.Gray, boundary.maxHPBoundary1P, barY, barWidth1P, barHeight);
                //2P
                g.FillRectangle(Brushes.Green, boundary.minHPBoundary2P, barY, barWidth2P, barHeight);
            }

        }
        private void UpdateHPDisplay() {
            pictureBoxFrame.Invalidate();
        }

        private void SetDisplayFrame(Bitmap newFrame) {
            pictureBoxFrame.Image?.Dispose();
            pictureBoxFrame.Image = newFrame;
        }

        public void PictureBoxBW_MouseClick(object sender, MouseEventArgs e) {
            selectedY = e.Y;
        }


        public void TrackBarFrame_Scroll(object sender, EventArgs e) {
            SetDisplayFrame(videoL.GetFrameRead(trackBarFrame.Value));
            FrameBox.Text = ($"Frame:{videoL.currentframe.ToString()}");
        }


        public void AnalyzeB_Click(object sender, EventArgs e) {
            string boundariesString = string.Join(", ", boundaries);
            MessageBox.Show(boundariesString, "Boundaries", MessageBoxButtons.OK, MessageBoxIcon.Information);
            boundary.SetBaseBoundaries(boundaries);
            listBox.Items.Add($"解析開始を押すと解析が開始されます。");
        }


        public async void CaliculateAllFramesB_Click(object sender, EventArgs e) {
            var progress = new Progress<int>(percent => this.progressBar.Value = percent);
            var result = await boundary.CaliculateAllFrameHP(progress, SF5, selectedY);
            if (result != null) {
                lastAnalysisResult = result;
                healthPercents1P = result.HP1P;
                healthPercents2P = result.HP2P;
                errorList = result.Errors;
                UpdateHPDisplay();
            }
        }

        public void SaveToCSVB_Click(object sender, EventArgs e) {
            if (lastAnalysisResult == null) {
                MessageBox.Show("解析結果がありません。先に解析を実行してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string directoryPath = Path.GetDirectoryName(filePath);
            string originalFileName = Path.GetFileNameWithoutExtension(filePath);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string csvFileName = $"{originalFileName}_{timestamp}.csv";
            string csvFilePath = Path.Combine(directoryPath, csvFileName);
            boundary.SaveHPPercentagesToCSV(csvFilePath, lastAnalysisResult);
            listBox.Items.Add($"保存先:{directoryPath},保存名:{csvFileName}");
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
                SetDisplayFrame(videoL.GetFrameRead(trackBarFrame.Value));
                trackBarFrame.Value++;
            } else {
                timerFramePlay.Enabled = false;
            }
        }

        private void trackBarFrame_ValueChanged(object sender, EventArgs e) {
            FrameBox.Text = ($"Frame:{videoL.currentframe.ToString()}");
        }

        private void buttonSF5_Click(object sender, EventArgs e) {
            SF5 = true;
            listBox.Items.Add($"SF5用の解析に切り替わりました。");
            listBox.Items.Add($"SF5の取得座標は、Y座標は固定ですが、座標設定を押して、適当なフレームで画面をクリックしてください。");
        }

        private void buttonSF6_Click(object sender, EventArgs e) {
            SF5 = false;
            listBox.Items.Add($"SF6用の解析に切り替わりました。");
            listBox.Items.Add($"座標設定を押して、適当なフレームで画面をクリックしてください。");
        }

        private void buttonClear_Click(object sender, EventArgs e) {
            listBox.Items.Clear();
        }


    }
}
