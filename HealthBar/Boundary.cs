using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        public int threshold = 30;
        public int thresholdSF5 = 40;
        public int tempBoundary1P = 0;
        public int maxHPBoundary2P = 0;
        public int minHPBoundary2P = 0;
        public int tempBoundary2P = 0;
        public int temp1P = 0;
        public int temp2P = 0;
        public int gradientCount1P;
        public int gradientCount2P;
        public int thresholdGradient = 4;
        public int boundary1P;
        public int boundary2P;

        public Boundary(HPBarForm form) {
            this.form = form;
        }
        //--------------------------------------------Street Fighter6--------------------------------------------
        public List<int> FindBoundary(List<int> gradient) {
            List<int> boundaries = new List<int>();
            int temp = 0;
            bool mode = true; // 境界内を見つけるモード
            int threshold = 20;
            int continuousPixels = 250; // 連続する「ほぼ0」領域の最小長さ
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
                        boundaries.Add(i - continuousPixels - 2);
                        mode = false;
                        temp = 0;
                        count++;
                    }
                } else {
                    if (diff > threshold) {
                        boundaries.Add(i + 1);
                        mode = true;
                        count++;
                    }
                }
            }
            return boundaries;
        }


        public string DetectBarState(Vec3b color) {
            int r = color.Item2, g = color.Item1, b = color.Item0;
            // 黄色バー
            if (r >= 100 && g >= 160 && b <= 200 && r >= b && g >= b) return "Yellow";
            // ダメージバー
            if (r >= 100 && g >= 100 && b <= 100 && b >= 30) return "Damage";
            // 1P通常バー
            if (r >= 100 && r <= 250 && g <= 50 && b >= 30 && b <= 150 && r >= b && b >= g) return "1PBar";
            // 2P通常バー
            if (g <= 220 && g >= 50 && b >= 100 && b >= g && g >= r) return "2PBar";
            // ノイズ
            return "noize";
        }

        public void AnalyzeBoundary1Pand2P(int currentFrame, int y) {
            Bitmap frameBitmap = form.videoL.GetFrameRead(currentFrame);
            Mat frame = OpenCvSharp.Extensions.BitmapConverter.ToMat(frameBitmap);
            try {
                //Gradientと同じ操作を行う
                Vec3b color;
                int prevIntensity = -threshold;
                int intensity;
                int gradient;
                gradientCount1P = 0;
                if (temp1P == 0) {
                    temp1P = maxHPBoundary1P - 1;
                }
                for (int x = maxHPBoundary1P - 1; x < minHPBoundary1P - 2; x++) {
                    color = frame.At<Vec3b>(y, x);
                    intensity = color.Item0 + color.Item1 + color.Item2;
                    gradient = Math.Abs(intensity - prevIntensity);
                    if (gradient > threshold && DetectBarState(color) == "1PBar") {
                        //体力25%より大きい
                        tempBoundary1P = x - 1;
                        Console.WriteLine($"1PBar{currentFrame},x{x}");
                        //Console.WriteLine($"Frame {currentFrame}: Boundary detected at {tempBoundary1P} with gradient {gradient},{gradientCount1P}.");
                    } else if (gradient > threshold && DetectBarState(color) == "Yellow") {
                        //体力25%以下
                        tempBoundary1P = x - 1;
                        Console.WriteLine($"Yellow{currentFrame},x{x}");
                    } else if (DetectBarState(color) == "noize" && x > temp1P) {
                        Console.WriteLine($"Noize{currentFrame},x{x}");
                        gradientCount1P++;
                    }
                    prevIntensity = intensity;
                }
                if (gradientCount1P > thresholdGradient) {
                    tempBoundary1P = temp1P;
                }

                //KOのKの字が見えて、かつ最後のXがNoizeなら自分のHPを0と判定する
                if (CheckKOandHPmin(y, frame, minHPBoundary1P, -3)) {
                    tempBoundary1P = minHPBoundary1P;
                    Console.WriteLine($"1PLostHP");
                }
                temp1P = tempBoundary1P;
                //2P
                prevIntensity = -threshold;
                if (temp2P == 0) {
                    temp2P = maxHPBoundary2P + 1;
                }
                gradientCount2P = 0;
                for (int x = maxHPBoundary2P + 1; x > minHPBoundary2P + 3; x--) {
                    color = frame.At<Vec3b>(y, x);
                    intensity = color.Item0 + color.Item1 + color.Item2;
                    gradient = Math.Abs(intensity - prevIntensity);
                    if (gradient > threshold && DetectBarState(color) == "2PBar") {
                        //体力25%より大きい
                        tempBoundary2P = x + 1;
                        Console.WriteLine($"2PBar{currentFrame},x{x}");
                        //Console.WriteLine($"Frame {currentFrame}: Boundary detected at {tempBoundary1P} with gradient {gradient},{gradientCount1P}.");
                    } else if (gradient > threshold && DetectBarState(color) == "Yellow") {
                        //体力25%以下
                        tempBoundary2P = x + 1;
                        Console.WriteLine($"Yellow{currentFrame},x{x}");
                    } else if (DetectBarState(color) == "noize" && x < temp2P) {
                        Console.WriteLine($"Noize{currentFrame},x{x}");
                        gradientCount2P++;
                    }
                    prevIntensity = intensity;
                }
                if (gradientCount2P > thresholdGradient) {
                    tempBoundary2P = temp2P;
                }
                //KOのKの字が見えて、かつ最後のXがNoizeなら自分のHPを0と判定する
                if (CheckKOandHPmin(y, frame, minHPBoundary2P, +3)) {
                    tempBoundary2P = minHPBoundary2P;
                    Console.WriteLine($"2PLostHP");
                }
                temp2P = tempBoundary2P;
                frame.Dispose();
                frameBitmap.Dispose();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                frame.Dispose();
                frameBitmap.Dispose();
            }
        }
        public async Task CaliculateAllFrameHP(IProgress<int> progress) {
            int currentBoundary1P = maxHPBoundary1P;
            int currentBoundary2P = maxHPBoundary2P;
            int baseHPWidth1P = Math.Abs(minHPBoundary1P - maxHPBoundary1P);
            int baseHPWidth2P = Math.Abs(minHPBoundary2P - maxHPBoundary2P);
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
                form.errorList.Clear();
                //iはフレーム番号
                for (int i = 0; i < form.videoL.TotalFrames; i++) {
                    //今のBoundaryをcurrentBoundaryとして表示
                    //フレーム番号iにおける画像のGradientを出し、それで計算を行う
                    if (form.SF5) {
                        AnalyzeBoundarySF5(i, form.selectedY);
                    } else {
                        AnalyzeBoundary1Pand2P(i, form.selectedY);
                    }
                    currentBoundary1P = tempBoundary1P;
                    currentBoundary2P = tempBoundary2P;


                    //現在の体力割合
                    currentHPWidth1P = Math.Abs(minHPBoundary1P - currentBoundary1P);
                    currentHPWidth2P = Math.Abs(minHPBoundary2P - currentBoundary2P);

                    // 体力割合を計算
                    hpPercentage1P = (double)currentHPWidth1P / baseHPWidth1P * 100;
                    hpPercentage1P = Math.Max(0, Math.Min(100, hpPercentage1P));
                    lock (form.healthPercents1P) {
                        form.healthPercents1P.Add(hpPercentage1P);
                    }
                    hpPercentage2P = (double)currentHPWidth2P / baseHPWidth2P * 100;
                    lock (form.healthPercents2P) {
                        form.healthPercents2P.Add(hpPercentage2P);
                    }
                    lock (form.errorList) {
                        if (gradientCount1P > thresholdGradient && gradientCount2P > thresholdGradient) {
                            form.errorList.Add("3");
                        } else if (gradientCount1P > thresholdGradient) {
                            form.errorList.Add("1");
                        } else if (gradientCount2P > thresholdGradient) { form.errorList.Add("2"); } else { form.errorList.Add("0"); }
                    }
                    Console.WriteLine($"Frame{i},{gradientCount1P},{gradientCount2P},x1P{tempBoundary1P},2P{tempBoundary2P}");
                    //進捗報告
                    progress.Report((i + 1) * 100 / form.videoL.TotalFrames);
                }
            });
            MessageBox.Show("全フレームの体力割合の計算が完了しました。", "計算完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            form.UpdateHPDisplay();
        }

        public bool CheckKOandHPmin(int y, Mat frame, int minHPBoundary, int offset) {
            bool allInRange = false;
            Vec3b color;
            color = frame.At<Vec3b>(y, minHPBoundary + offset);
            if (DetectBarState(color) == "noize") {
                Console.WriteLine($"NoizeClear");
                allInRange = true;
            } else {
                Console.WriteLine($"NoizeFailure");
                allInRange = false;
                return allInRange;
            }
            for (int x = 200; x <= 250; x++) {
                color = frame.At<Vec3b>(y, x);
                int r = color.Item2; // R成分
                int g = color.Item1; // G成分
                int b = color.Item0; // B成分
                int rgbMin = 200;
                int rgbMax = 250;

                // RGB値が指定範囲外の場合
                if (r < rgbMin || r > rgbMax || g < rgbMin || g > rgbMax || b < rgbMin || b > rgbMax) {
                    allInRange = false;
                    Console.WriteLine($"KOFailure");
                    break; // 一つでも条件を満たさなければループを抜ける
                }
            }
            return allInRange;
        }
        //--------------------------------------------ここまでStreet Fighter6--------------------------------------------
        //--------------------------------------------ここからStreet Fighter5--------------------------------------------
        public List<int> FindBoundarySF5(int currentFrame, int y) {
            Bitmap frameBitmap = form.videoL.GetFrameRead(currentFrame);
            Mat frame = OpenCvSharp.Extensions.BitmapConverter.ToMat(frameBitmap);
            List<int> boundaries = new List<int>();
            Vec3b color;
            //1P側
            for (int x = 200; x > 0; x--) {
                color = frame.At<Vec3b>(y, x);
                if (color.Item2 < 150) {
                    boundaries.Add(x + 1);
                    break;
                }
                Console.WriteLine(x.ToString());
            }
            for (int x = 200; x < 700; x++) {
                color = frame.At<Vec3b>(y, x);
                if (color.Item2 < 150) {
                    boundaries.Add(x - 1);
                    break;
                }
            }
            //2P側
            for (int x = 800; x > 0; x--) {
                color = frame.At<Vec3b>(y, x);
                if (color.Item2 < 150) {
                    boundaries.Add(x + 1);
                    break;
                }
            }
            for (int x = 800; x < 1200; x++) {
                color = frame.At<Vec3b>(y, x);
                if (color.Item2 < 150) {
                    boundaries.Add(x - 1);
                    break;
                }
            }
            frame.Dispose();
            frameBitmap.Dispose();
            return boundaries;
        }
        public string DetectBarStateSF5(Vec3b color) {
            int r = color.Item2, g = color.Item1, b = color.Item0;
            //黄色バー
            if (r > 200 && g > 170 && b > 70 && b < 200) return "YellowBar";
            if (r > 180 && g > 150 && b > 70 && b < 220) return "YellowBar";
            // 体力があったとされるところ
            if (r <= 100 && g <= 100 && b <= 100 && Math.Abs(r - g) <= 50) return "NoHealth";
            // 通常バー
            if (r > 50 && g > 180 && b < 200) return "PlayerBar";
            //通常バー２
            if (r > 150 && g > 100 && g < 220 && b > 80 && b < 150) return "PlayerBar";
            //通常バー３、GreenEdge
            if (r < 100 && g > 220 && b > 70 && b < 120) return "PlayerBar";
            // Damageと通常バーの境目
            if (r > 200 && g < 120 && b < 100) return "PlayerDamagedLine";
            // ダメージバー
            if (r >= 100 && g <= 100 && b <= 100) return "Damage";
            // 削れているところ
            if (r >= 100 && g >= 100 && b >= 100 && r <= 150 && g <= 150 && b <= 150 && Math.Abs(r - g) <= 50 && Math.Abs(b - g) <= 50) return "TempDamage";
            //最初らへんのバーのところ
            if (Math.Abs(r - g) < 20 && Math.Abs(g - b) < 20) return "Edge";
            // ノイズ
            return "noize";
        }

        public void AnalyzeBoundarySF5(int currentFrame, int y) {
            Bitmap frameBitmap = form.videoL.GetFrameRead(currentFrame);
            Mat frame = OpenCvSharp.Extensions.BitmapConverter.ToMat(frameBitmap);
            try {
                //Gradientと同じ操作を行う
                Vec3b color;
                int prevIntensity = -threshold;
                int intensity;
                int gradient;
                gradientCount1P = 0;
                if (temp1P == 0) {
                    temp1P = maxHPBoundary1P - 1;
                }
                for (int x = maxHPBoundary1P; x < minHPBoundary1P - 2; x++) {
                    color = frame.At<Vec3b>(y, x);
                    intensity = color.Item0 + color.Item1 + color.Item2;
                    gradient = Math.Abs(intensity - prevIntensity);
                    if (gradient > threshold) {
                        Console.WriteLine($"1Pthreshold{currentFrame},x{x},gra{gradient},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp1P},{DetectBarStateSF5(color)}");
                    }
                    if (DetectBarStateSF5(color) == "YellowBar" && maxHPBoundary1P == x - 1) {
                        //体力100%
                        tempBoundary1P = x - 1;
                        Console.WriteLine($"MAX1PBar{currentFrame},x{x},gra{gradient},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp1P},{DetectBarStateSF5(color)}");
                    } else if (gradient > thresholdSF5 && Math.Abs(temp1P - x) > (minHPBoundary1P - maxHPBoundary1P) / 2 && x > temp1P) {
                        gradientCount1P = 5;
                        Console.WriteLine($"1PRapid HPdown{currentFrame},x{x},gra{gradient},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp1P},{DetectBarStateSF5(color)}");
                    } else if (gradient > thresholdSF5 && (DetectBarStateSF5(color) == "PlayerBar" || DetectBarStateSF5(color) == "PlayerDamagedLine" || DetectBarStateSF5(color) == "YellowBar")) {
                        tempBoundary1P = x - 1;
                        Console.WriteLine($"1PBar{currentFrame},x{x},gra{gradient},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp1P},{DetectBarStateSF5(color)}");
                    } else if (gradient < thresholdSF5 && DetectBarStateSF5(color) == "YellowBar") {
                        //Console.WriteLine($"1PYellowBar{currentFrame},x{x},gra{gradient},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp1P}");
                    } else if (DetectBarStateSF5(color) == "noize" && x > temp1P) {
                        Console.WriteLine($"Noize{currentFrame},x{x},gra{gradient},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp1P},{DetectBarStateSF5(color)}");
                        gradientCount1P++;
                    } else if (gradient > thresholdSF5 && DetectBarStateSF5(color) == "NoHealth" && x > temp1P) {
                        Console.WriteLine($"NoizeNoHealth1P{currentFrame},x{x},gra{gradient},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp1P},{DetectBarStateSF5(color)}");
                        gradientCount1P++;
                    }
                    prevIntensity = intensity;
                }
                color = frame.At<Vec3b>(y, minHPBoundary1P);
                intensity = color.Item0 + color.Item1 + color.Item2;
                Console.WriteLine($"1Pfinal{currentFrame},x{minHPBoundary1P},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp2P},{DetectBarStateSF5(color)}");
                if (Math.Abs(minHPBoundary1P  + maxHPBoundary1P * 3) / 4 < temp1P && tempBoundary1P < Math.Abs(minHPBoundary1P*3 + maxHPBoundary1P ) / 4) {
                    tempBoundary1P = minHPBoundary1P;
                } 
                if (gradientCount2P >= thresholdGradient) {
                    tempBoundary2P = temp2P;

                }
                if (gradientCount1P > thresholdGradient) {
                    tempBoundary1P = temp1P;
                }
                temp1P = tempBoundary1P;
                //2P
                prevIntensity = -threshold;
                if (temp2P == 0) {
                    temp2P = maxHPBoundary2P + 1;
                }
                gradientCount2P = 0;
                for (int x = maxHPBoundary2P; x > minHPBoundary2P + 2; x--) {
                    color = frame.At<Vec3b>(y, x);
                    intensity = color.Item0 + color.Item1 + color.Item2;
                    gradient = Math.Abs(intensity - prevIntensity);
                    if (gradient > threshold) {
                        Console.WriteLine($"2Pthreshold{currentFrame},x{x},gra{gradient},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp2P},{DetectBarStateSF5(color)}");
                    }
                    if (DetectBarStateSF5(color) == "YellowBar" && maxHPBoundary2P == x + 1) {
                        //体力100%
                        tempBoundary2P = x + 1;
                        Console.WriteLine($"MAX2PBar{currentFrame},x{x},gra{gradient},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp2P},{DetectBarStateSF5(color)}");
                    } else if (gradient > thresholdSF5 && Math.Abs(temp2P - x) > (maxHPBoundary2P - minHPBoundary2P) / 2 && x < temp2P) {
                        gradientCount2P = 5;
                        Console.WriteLine($"2PRapid HPdown{currentFrame},x{x},gra{gradient},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp1P},{DetectBarStateSF5(color)}");
                    } else if (gradient < thresholdSF5 && DetectBarStateSF5(color) == "YellowBar") {
                        //Console.WriteLine($"2PYellowBar{currentFrame},x{x},gra{gradient},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp1P}");
                    } else if (gradient > thresholdSF5 && (DetectBarStateSF5(color) == "PlayerBar" || DetectBarStateSF5(color) == "PlayerDamagedLine" || DetectBarStateSF5(color) == "YellowBar")) {
                        tempBoundary2P = x + 1;
                        Console.WriteLine($"2PBar{currentFrame},x{x},gra{gradient},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp2P},{DetectBarStateSF5(color)}");
                    } else if (DetectBarStateSF5(color) == "noize" && x < temp2P) {
                        Console.WriteLine($"Noize{currentFrame},x{x},gra{gradient},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp2P},{DetectBarStateSF5(color)}");
                        gradientCount2P++;
                    } else if (gradient > thresholdSF5 && DetectBarStateSF5(color) == "NoHealth" && x < temp2P) {
                        Console.WriteLine($"NoizeNoHealth2P{currentFrame},x{x},gra{gradient},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp1P},{DetectBarStateSF5(color)}");
                        gradientCount2P++;
                    }
                    prevIntensity = intensity;
                }
                color = frame.At<Vec3b>(y, minHPBoundary2P);
                intensity = color.Item0 + color.Item1 + color.Item2;
                Console.WriteLine($"2Pfinal{currentFrame},x{minHPBoundary2P},r{color.Item2},g{color.Item1},b{color.Item0},temp{temp2P},{DetectBarStateSF5(color)}");
                if (Math.Abs(minHPBoundary2P * 3 + maxHPBoundary2P) / 4 > temp2P && tempBoundary2P > Math.Abs(minHPBoundary2P + maxHPBoundary2P * 3) / 4) {
                    tempBoundary2P = minHPBoundary2P;
                    Console.WriteLine($"minMax{Math.Abs(minHPBoundary2P * 3 + maxHPBoundary2P) / 4},temp2P{temp2P},{Math.Abs(minHPBoundary2P + maxHPBoundary2P * 3) / 4}");
                } 
                if (gradientCount2P >= thresholdGradient) {
                    tempBoundary2P = temp2P;

                }
                temp2P = tempBoundary2P;
                frame.Dispose();
                frameBitmap.Dispose();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                frame.Dispose();
                frameBitmap.Dispose();
            }
        }










        //--------------------------------------------ここまでStreet Fighter5--------------------------------------------
        //--------------------------------------------ここから共通事項--------------------------------------------
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
        public void SaveHPPercentagesToCSV(string outputPath) {
            using (StreamWriter sw = new StreamWriter(outputPath)) {
                sw.WriteLine("FrameNumber,LeftHP[%],RightHP[%],ErrorInformation");
                for (int i = 0; i < form.healthPercents1P.Count; i++) {
                    sw.WriteLine($"{i},{form.healthPercents1P[i]},{form.healthPercents2P[i]},{form.errorList[i]}");
                }
            }

            MessageBox.Show("体力割合をCSVに保存しました。", "保存完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
