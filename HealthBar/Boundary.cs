using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace HealthBar {
    public class Boundary {
        public HPBarForm form;
        public int maxHPBoundary = 0;
        public int minHPBoundary = 0;
        public int threshold = 100;
        public int tempBoundary = 0;
        VideoLoader videoL;
        public Boundary(HPBarForm form) {
            this.form = form;
            this.videoL = new VideoLoader();
        }
        public void LoadVideo(string filePath) {
            if (!videoL.LoadVideo(filePath)) {
                throw new Exception("ロード失敗");
            }
        }
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

        //gradient(currentFrame, Y座標)
        //ボツ、隠れるとすぐ座標おかしくなる
        //public int AnalyzeBoundary(List<int> gradient, int currentFrame, int y) {
        //    bool breakpoint = false;
        //    //前回の境界の位置から緑の値が大きくなっていたら検出開始にする（黄色のゲージ消費のイメージ）
        //    var (rValues, gValues, bValues) = form.caliculate.GetRGB(currentFrame, y);
        //    if (gradient.All(value => value <= 100)) {
        //        tempBoundary = maxHPBoundary;
        //        Console.WriteLine($"Frame {currentFrame}: Screen is dark. Resetting currentBoundary to {tempBoundary}.");
        //        return tempBoundary;
        //    }

        //    //G成分のチェック
        //    if (gValues[tempBoundary] > threshold && rValues[tempBoundary] > threshold) {
        //        for (int x = tempBoundary + 1; x < minHPBoundary; x++) {
        //            //境界設定
        //            if (gradient[x] > threshold && gValues[x] > threshold) {
        //                tempBoundary = x;
        //                breakpoint = true;
        //                break;
        //            }
        //            if (breakpoint) { break; }
        //        }
        //        if (!breakpoint) {
        //            return minHPBoundary;
        //        } else {
        //        }
        //    }return tempBoundary;
        //}
        public int AnalyzeBoundary(List<int> gradient, int currentFrame, int y) {
            bool breakpoint = false;
            //
            var (rValues, gValues, bValues) = form.caliculate.GetRGB(currentFrame, y);
            //暗転時のリセット
            if (gradient.All(value => value <= 100)) {
                tempBoundary = maxHPBoundary;
                Console.WriteLine($"Frame {currentFrame}: Screen is dark. Resetting currentBoundary to {tempBoundary}.");
                return tempBoundary;
            }

            //G成分のチェック
            if (gValues[tempBoundary] > threshold && rValues[tempBoundary] > threshold) {
                for (int x = minHPBoundary -1; x < tempBoundary; x--) {
                    //境界設定
                    if (gradient[x] > threshold ) {
                        tempBoundary = x;
                        breakpoint = true;
                        break;
                    }
                    if (breakpoint) {
                        breakpoint = false;
                        break; }
                }
                if (!breakpoint) {
                    return minHPBoundary;
                } else {
                }
            }
            return tempBoundary;
        }
        public void SetBaseBoundaries(List<int> boundaries) {
            if (boundaries.Count >= 2) {
                maxHPBoundary = boundaries[0];
                minHPBoundary = boundaries[1];
                tempBoundary = maxHPBoundary;
                MessageBox.Show($"基準フレームの境界が設定されました。左端: {maxHPBoundary}, 右端: {minHPBoundary}", "基準設定", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                MessageBox.Show("境界が正しく検出されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public async Task CaliculateAllFrameHP(IProgress<int> progress) {
            int baseHPWidth = minHPBoundary - maxHPBoundary;
            int currentHPWidth = minHPBoundary-maxHPBoundary;
            double hpPercentage = (double)currentHPWidth / baseHPWidth * 100;
            if (maxHPBoundary == 0 && minHPBoundary == 0) {
                MessageBox.Show("基準フレームが設定されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await Task.Run(() => {
                //100%とする体力ゲージの設定
                form.healthPercents.Clear();
                for (int i = 0; i < videoL.TotalFrames; i++) {
                    //今のBoundaryをcurrentBoundaryとして表示
                    int currentBoundary = AnalyzeBoundary(form.caliculate.Gradient1(i, form.selectedY), i, form.selectedY);
                    currentHPWidth = minHPBoundary - currentBoundary;

                    // 体力割合を計算
                    hpPercentage = (double)currentHPWidth / baseHPWidth * 100;
                    lock (form.healthPercents) {
                        form.healthPercents.Add(hpPercentage);
                    }
                    //進捗報告
                    progress.Report(i * 100 / videoL.TotalFrames);
                }
            });
            MessageBox.Show("全フレームの体力割合の計算が完了しました。", "計算完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void SaveHPPercentagesToCSV(string outputPath) {
            using (StreamWriter sw = new StreamWriter(outputPath)) {
                sw.WriteLine("Frame,HPPercentage");
                for (int i = 0; i < form.healthPercents.Count; i++) {
                    sw.WriteLine($"{i},{form.healthPercents[i]}");
                }
            }

            MessageBox.Show("体力割合をCSVに保存しました。", "保存完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
