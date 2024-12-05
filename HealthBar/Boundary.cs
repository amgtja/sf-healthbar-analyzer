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
        public int maxHPBoundary1P = 0;
        public int minHPBoundary1P = 0;
        public int threshold = 100;
        public int tempBoundary1P = 0;
        public int maxHPBoundary2P = 0;
        public int minHPBoundary2P = 0;
        public int tempBoundary2P = 0;
        public Boundary(HPBarForm form) {
            this.form = form;
        }
        public List<int> FindBoundary(List<int> gradient) {
            List<int> boundaries = new List<int>();
            int temp = 0;
            bool mode = true; // 境界内を見つけるモード
            int threshold = 100; // 閾値を大きめに設定
            int continuousPixels = 120; // 連続する「ほぼ0」領域の最小長さ
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
        //public int AnalyzeBoundary1P(List<int> gradient, int currentFrame, int y) {
        //    bool breakpoint = false;
        //    //前回の境界の位置から緑の値が大きくなっていたら検出開始にする（黄色のゲージ消費のイメージ）
        //    var (rValues, gValues, bValues) = form.caliculate.GetRGB(currentFrame, y);
        //    if (gradient.All(value => value <= 100)) {
        //        tempBoundary1P = maxHPBoundary1P;
        //        Console.WriteLine($"Frame {currentFrame}: Screen is dark. Resetting currentBoundary to {tempBoundary1P}.");
        //        return tempBoundary1P;
        //    }

        //    //G成分のチェック
        //    if (gValues[tempBoundary1P] > threshold && rValues[tempBoundary1P] > threshold) {
        //        for (int x = tempBoundary1P + 1; x < minHPBoundary1P; x++) {
        //            //境界設定
        //            if (gradient[x] > threshold && gValues[x] > threshold) {
        //                tempBoundary1P = x;
        //                breakpoint = true;
        //                break;
        //            }
        //            if (breakpoint) { break; }
        //        }
        //        if (!breakpoint) {
        //            return minHPBoundary1P;
        //        } else {
        //        }
        //    }return tempBoundary1P;
        //}
        public int AnalyzeBoundary1P(List<int> gradient, int currentFrame, int y) {
            // 暗転時のリセット処理：gradient の全ての値が100以下の場合はリセット
            if (gradient.All(value => value <= 100)) {
                tempBoundary1P = maxHPBoundary1P;
                //Console.WriteLine($"Frame {currentFrame}: Screen is dark. Resetting currentBoundary to {tempBoundary1P}.");
                return tempBoundary1P;
            }

            // minHPBoundary1P から maxHPBoundary1P の方向へスライドして gradient を確認
            for (int x = minHPBoundary1P - maxHPBoundary1P-5; x > 0; x--) {
                // gradient が閾値を超えた場合にその位置を境界として設定
                if (gradient[x] > threshold) {
                    tempBoundary1P = x;
                    //Console.WriteLine($"Frame {currentFrame}: Boundary detected at {tempBoundary1P} with gradient {gradient[x]}.");
                    return tempBoundary1P;
                }
            }
            // 何も検出されなかった場合は、最小の境界を返す
            return minHPBoundary1P;
        }
        public int AnalyzeBoundary2P(List<int> gradient, int currentFrame, int y) {
            // 暗転時のリセット処理：gradient の全ての値が100以下の場合はリセット
            if (gradient.All(value => value <= 100)) {
                tempBoundary2P = maxHPBoundary2P;
                //Console.WriteLine($"Frame {currentFrame}: Screen is dark. Resetting currentBoundary to {tempBoundary2P}.");
                return tempBoundary2P;
            }

            // minHPBoundary2P から maxHPBoundary2P の方向へスライドして gradient を確認
            for (int x = 5;x<maxHPBoundary2P-minHPBoundary2P;x++) {
                // gradient が閾値を超えた場合にその位置を境界として設定
                if (gradient[x] > threshold) {
                    tempBoundary1P = x;
                    //Console.WriteLine($"Frame {currentFrame}: Boundary detected at {tempBoundary2P} with gradient {gradient[x]}.");
                    return tempBoundary2P;
                }
            }
            // 何も検出されなかった場合は、最小の境界を返す
            return minHPBoundary2P;
        }
        public void SetBaseBoundaries(List<int> boundaries) {
            if (boundaries.Count >= 2) {
                maxHPBoundary1P = boundaries[0];
                minHPBoundary1P = boundaries[1];
                maxHPBoundary2P = boundaries[3];
                minHPBoundary2P = boundaries[2];
                tempBoundary1P = maxHPBoundary1P;
                MessageBox.Show($"基準フレームの境界 1P: {maxHPBoundary1P} - {minHPBoundary1P}, 2P: {minHPBoundary2P} - {maxHPBoundary2P}", "基準設定", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                MessageBox.Show("境界が正しく検出されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public async Task CaliculateAllFrameHP(IProgress<int> progress) {
            int currentBoundary1P = maxHPBoundary1P;
            int currentBoundary2P = maxHPBoundary2P;
            int baseHPWidth1P = minHPBoundary1P - maxHPBoundary1P;
            int baseHPWidth2P = maxHPBoundary2P - minHPBoundary2P;
            int currentHPWidth1P = baseHPWidth1P;
            int currentHPWidth2P = baseHPWidth2P;
            double hpPercentage1P = 100.0;
            double hpPercentage2P = 100.0;
            if (maxHPBoundary1P == 0 && minHPBoundary1P == 0) {
                MessageBox.Show("基準フレームが設定されていません。(1P)", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (maxHPBoundary2P == 0 && minHPBoundary2P == 0) {
                MessageBox.Show("基準フレームが設定されていません。(2P)", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await Task.Run(() => {
                //100%とする体力ゲージの設定
                form.healthPercents1P.Clear();
                form.healthPercents2P.Clear();
                //iはフレーム番号
                for (int i = 0; i < form.videoL.TotalFrames; i++) {
                    //今のBoundaryをcurrentBoundaryとして表示
                    //フレーム番号iにおける画像のGradientを出し、それで計算を行う
                    currentBoundary1P = AnalyzeBoundary1P(form.caliculate.GradientNarrow(i, form.selectedY, maxHPBoundary1P,minHPBoundary1P), i, form.selectedY);
                    currentBoundary2P = AnalyzeBoundary2P(form.caliculate.GradientNarrow(i, form.selectedY, minHPBoundary2P, maxHPBoundary2P), i, form.selectedY);

                    //現在の体力割合
                    currentHPWidth1P = minHPBoundary1P - maxHPBoundary1P;
                    currentHPWidth2P = maxHPBoundary2P - minHPBoundary2P;

                    // 体力割合を計算
                    hpPercentage1P = (double)currentHPWidth1P / baseHPWidth1P * 100;
                    hpPercentage1P = Math.Max(0, Math.Min(100, hpPercentage1P));
                    lock (form.healthPercents1P) {
                        form.healthPercents1P.Add(hpPercentage1P);
                    }
                    hpPercentage2P = (double)currentHPWidth2P / baseHPWidth2P * 100;
                    hpPercentage2P = Math.Max(0, Math.Min(100, hpPercentage2P));
                    lock (form.healthPercents2P) {
                        form.healthPercents2P.Add(hpPercentage2P);
                    }

                    //進捗報告
                    progress.Report((i + 1) * 100 / form.videoL.TotalFrames);
                }
            });
            MessageBox.Show("全フレームの体力割合の計算が完了しました。", "計算完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            form.UpdateHPDisplay();
        }
        public void SaveHPPercentagesToCSV(string outputPath) {
            using (StreamWriter sw = new StreamWriter(outputPath)) {
                sw.WriteLine("Frame,HP1P,HP2P");
                for (int i = 0; i < form.healthPercents1P.Count; i++) {
                    sw.WriteLine($"{i},{form.healthPercents1P[i]},{form.healthPercents2P[i]}");
                }
            }

            MessageBox.Show("体力割合をCSVに保存しました。", "保存完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
