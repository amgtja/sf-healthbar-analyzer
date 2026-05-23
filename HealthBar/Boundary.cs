using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace HealthBar {
    public class Boundary {
        private readonly IFrameReader frameReader;
        public int maxHPBoundary1P = 0;
        public int minHPBoundary1P = 0;
        public int maxHPBoundary2P = 0;
        public int minHPBoundary2P = 0;
        private int threshold = 30;
        private int thresholdSF5 = 40;
        private int tempBoundary1P = 0;
        private int tempBoundary2P = 0;
        private int temp1P = 0;
        private int temp2P = 0;
        private int gradientCount1P;
        private int gradientCount2P;
        private int thresholdGradient = 4;

        public event Action<string> OnMessage;

        // SF6: horizontal gradient scan constants
        private const int Sf6GradientThreshold = 20;
        private const int Sf6MinBarWidthPixels = 250;

        // SF5: approximate bar positions at 1280x720 resolution
        private const int Sf5Bar1PSearchOrigin = 200;
        private const int Sf5Bar1PRightEdge = 700;
        private const int Sf5Bar2PSearchOrigin = 800;
        private const int Sf5Bar2PRightEdge = 1200;
        private const int Sf5ColorThreshold = 150;

        // KO detection: pixel region where KO text appears on screen
        private const int KoTextRegionStart = 200;
        private const int KoTextRegionEnd = 250;
        private const int KoPixelIntensityMin = 200;
        private const int KoPixelIntensityMax = 250;

        public Boundary(IFrameReader frameReader) {
            this.frameReader = frameReader;
        }

        //--------------------------------------------Street Fighter6--------------------------------------------
        public List<int> FindBoundary(List<int> gradient) {
            List<int> boundaries = new List<int>();
            int temp = 0;
            bool mode = true;
            int count = 0;
            for (int i = 1; i < gradient.Count; i++) {
                if (count == 4) { break; }
                int diff = Math.Abs(gradient[i] - gradient[i - 1]);
                if (mode) {
                    if (diff < Sf6GradientThreshold) {
                        temp++;
                    } else {
                        temp = 0;
                    }
                    if (temp >= Sf6MinBarWidthPixels) {
                        boundaries.Add(i - Sf6MinBarWidthPixels - 2);
                        mode = false;
                        temp = 0;
                        count++;
                    }
                } else {
                    if (diff > Sf6GradientThreshold) {
                        boundaries.Add(i + 1);
                        mode = true;
                        count++;
                    }
                }
            }
            return boundaries;
        }

        public void AnalyzeBoundary1Pand2P(int currentFrame, int y) {
            Bitmap frameBitmap = frameReader.GetFrameRead(currentFrame);
            Mat frame = OpenCvSharp.Extensions.BitmapConverter.ToMat(frameBitmap);
            try {
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
                    if (gradient > threshold && BarColorClassifier.DetectBarState(color) == "1PBar") {
                        tempBoundary1P = x - 1;
                    } else if (gradient > threshold && BarColorClassifier.DetectBarState(color) == "Yellow") {
                        tempBoundary1P = x - 1;
                    } else if (BarColorClassifier.DetectBarState(color) == "noize" && x > temp1P) {
                        gradientCount1P++;
                    }
                    prevIntensity = intensity;
                }
                if (gradientCount1P > thresholdGradient) {
                    tempBoundary1P = temp1P;
                }
                if (CheckKOandHPmin(y, frame, minHPBoundary1P, -3)) {
                    tempBoundary1P = minHPBoundary1P;
                }
                temp1P = tempBoundary1P;

                // 2P
                prevIntensity = -threshold;
                if (temp2P == 0) {
                    temp2P = maxHPBoundary2P + 1;
                }
                gradientCount2P = 0;
                for (int x = maxHPBoundary2P + 1; x > minHPBoundary2P + 3; x--) {
                    color = frame.At<Vec3b>(y, x);
                    intensity = color.Item0 + color.Item1 + color.Item2;
                    gradient = Math.Abs(intensity - prevIntensity);
                    if (gradient > threshold && BarColorClassifier.DetectBarState(color) == "2PBar") {
                        tempBoundary2P = x + 1;
                    } else if (gradient > threshold && BarColorClassifier.DetectBarState(color) == "Yellow") {
                        tempBoundary2P = x + 1;
                    } else if (BarColorClassifier.DetectBarState(color) == "noize" && x < temp2P) {
                        gradientCount2P++;
                    }
                    prevIntensity = intensity;
                }
                if (gradientCount2P > thresholdGradient) {
                    tempBoundary2P = temp2P;
                }
                if (CheckKOandHPmin(y, frame, minHPBoundary2P, +3)) {
                    tempBoundary2P = minHPBoundary2P;
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

        public async Task<AnalysisResult> CaliculateAllFrameHP(IProgress<int> progress, bool isSF5, int selectedY) {
            int currentBoundary1P = maxHPBoundary1P;
            int currentBoundary2P = maxHPBoundary2P;
            int baseHPWidth1P = Math.Abs(minHPBoundary1P - maxHPBoundary1P);
            int baseHPWidth2P = Math.Abs(minHPBoundary2P - maxHPBoundary2P);
            int currentHPWidth1P = baseHPWidth1P;
            int currentHPWidth2P = baseHPWidth2P;
            double hpPercentage1P = 100.0;
            double hpPercentage2P = 100.0;
            if (maxHPBoundary1P == 0 && minHPBoundary1P == 0) {
                OnMessage?.Invoke("基準フレームが設定されていません。(1P)");
                return null;
            }
            if (maxHPBoundary2P == 0 && minHPBoundary2P == 0) {
                OnMessage?.Invoke("基準フレームが設定されていません。(2P)");
                return null;
            }

            var result = new AnalysisResult();
            await Task.Run(() => {
                for (int i = 0; i < frameReader.TotalFrames; i++) {
                    if (isSF5) {
                        AnalyzeBoundarySF5(i, selectedY);
                    } else {
                        AnalyzeBoundary1Pand2P(i, selectedY);
                    }
                    currentBoundary1P = tempBoundary1P;
                    currentBoundary2P = tempBoundary2P;

                    currentHPWidth1P = Math.Abs(minHPBoundary1P - currentBoundary1P);
                    currentHPWidth2P = Math.Abs(minHPBoundary2P - currentBoundary2P);

                    hpPercentage1P = (double)currentHPWidth1P / baseHPWidth1P * 100;
                    hpPercentage1P = Math.Max(0, Math.Min(100, hpPercentage1P));
                    lock (result.HP1P) {
                        result.HP1P.Add(hpPercentage1P);
                    }
                    hpPercentage2P = (double)currentHPWidth2P / baseHPWidth2P * 100;
                    lock (result.HP2P) {
                        result.HP2P.Add(hpPercentage2P);
                    }
                    lock (result.Errors) {
                        if (gradientCount1P > thresholdGradient && gradientCount2P > thresholdGradient) {
                            result.Errors.Add("3");
                        } else if (gradientCount1P > thresholdGradient) {
                            result.Errors.Add("1");
                        } else if (gradientCount2P > thresholdGradient) {
                            result.Errors.Add("2");
                        } else {
                            result.Errors.Add("0");
                        }
                    }
                    progress.Report((i + 1) * 100 / frameReader.TotalFrames);
                }
            });
            OnMessage?.Invoke("全フレームの体力割合の計算が完了しました。");
            return result;
        }

        public bool CheckKOandHPmin(int y, Mat frame, int minHPBoundary, int offset) {
            bool allInRange = false;
            Vec3b color;
            color = frame.At<Vec3b>(y, minHPBoundary + offset);
            if (BarColorClassifier.DetectBarState(color) == "noize") {
                allInRange = true;
            } else {
                return false;
            }
            for (int x = KoTextRegionStart; x <= KoTextRegionEnd; x++) {
                color = frame.At<Vec3b>(y, x);
                int r = color.Item2;
                int g = color.Item1;
                int b = color.Item0;
                if (r < KoPixelIntensityMin || r > KoPixelIntensityMax ||
                    g < KoPixelIntensityMin || g > KoPixelIntensityMax ||
                    b < KoPixelIntensityMin || b > KoPixelIntensityMax) {
                    allInRange = false;
                    break;
                }
            }
            return allInRange;
        }

        //--------------------------------------------ここまでStreet Fighter6--------------------------------------------
        //--------------------------------------------ここからStreet Fighter5--------------------------------------------
        public List<int> FindBoundarySF5(int currentFrame, int y) {
            Bitmap frameBitmap = frameReader.GetFrameRead(currentFrame);
            Mat frame = OpenCvSharp.Extensions.BitmapConverter.ToMat(frameBitmap);
            List<int> boundaries = new List<int>();
            Vec3b color;
            // 1P側
            for (int x = Sf5Bar1PSearchOrigin; x > 0; x--) {
                color = frame.At<Vec3b>(y, x);
                if (color.Item2 < Sf5ColorThreshold) {
                    boundaries.Add(x + 1);
                    break;
                }
            }
            for (int x = Sf5Bar1PSearchOrigin; x < Sf5Bar1PRightEdge; x++) {
                color = frame.At<Vec3b>(y, x);
                if (color.Item2 < Sf5ColorThreshold) {
                    boundaries.Add(x - 1);
                    break;
                }
            }
            // 2P側
            for (int x = Sf5Bar2PSearchOrigin; x > 0; x--) {
                color = frame.At<Vec3b>(y, x);
                if (color.Item2 < Sf5ColorThreshold) {
                    boundaries.Add(x + 1);
                    break;
                }
            }
            for (int x = Sf5Bar2PSearchOrigin; x < Sf5Bar2PRightEdge; x++) {
                color = frame.At<Vec3b>(y, x);
                if (color.Item2 < Sf5ColorThreshold) {
                    boundaries.Add(x - 1);
                    break;
                }
            }
            frame.Dispose();
            frameBitmap.Dispose();
            return boundaries;
        }

        public void AnalyzeBoundarySF5(int currentFrame, int y) {
            Bitmap frameBitmap = frameReader.GetFrameRead(currentFrame);
            Mat frame = OpenCvSharp.Extensions.BitmapConverter.ToMat(frameBitmap);
            try {
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
                    if (BarColorClassifier.DetectBarStateSF5(color) == "YellowBar" && maxHPBoundary1P == x - 1) {
                        tempBoundary1P = x - 1;
                    } else if (gradient > thresholdSF5 && Math.Abs(temp1P - x) > (minHPBoundary1P - maxHPBoundary1P) / 2 && x > temp1P) {
                        gradientCount1P = 5;
                    } else if (gradient > thresholdSF5 && (BarColorClassifier.DetectBarStateSF5(color) == "PlayerBar" || BarColorClassifier.DetectBarStateSF5(color) == "PlayerDamagedLine" || BarColorClassifier.DetectBarStateSF5(color) == "YellowBar")) {
                        tempBoundary1P = x - 1;
                    } else if (BarColorClassifier.DetectBarStateSF5(color) == "noize" && x > temp1P) {
                        gradientCount1P++;
                    } else if (gradient > thresholdSF5 && BarColorClassifier.DetectBarStateSF5(color) == "NoHealth" && x > temp1P) {
                        gradientCount1P++;
                    }
                    prevIntensity = intensity;
                }
                if (Math.Abs(minHPBoundary1P * 3 + maxHPBoundary1P) / 4 < temp1P && tempBoundary1P < Math.Abs(minHPBoundary1P + maxHPBoundary1P * 3) / 4) {
                    tempBoundary1P = minHPBoundary1P;
                }
                if (gradientCount1P > thresholdGradient) {
                    tempBoundary1P = temp1P;
                }
                temp1P = tempBoundary1P;

                // 2P
                prevIntensity = -threshold;
                if (temp2P == 0) {
                    temp2P = maxHPBoundary2P + 1;
                }
                gradientCount2P = 0;
                for (int x = maxHPBoundary2P; x > minHPBoundary2P + 2; x--) {
                    color = frame.At<Vec3b>(y, x);
                    intensity = color.Item0 + color.Item1 + color.Item2;
                    gradient = Math.Abs(intensity - prevIntensity);
                    if (BarColorClassifier.DetectBarStateSF5(color) == "YellowBar" && maxHPBoundary2P == x + 1) {
                        tempBoundary2P = x + 1;
                    } else if (gradient > thresholdSF5 && Math.Abs(temp2P - x) > (maxHPBoundary2P - minHPBoundary2P) / 2 && x < temp2P) {
                        gradientCount2P = 5;
                    } else if (gradient > thresholdSF5 && (BarColorClassifier.DetectBarStateSF5(color) == "PlayerBar" || BarColorClassifier.DetectBarStateSF5(color) == "PlayerDamagedLine" || BarColorClassifier.DetectBarStateSF5(color) == "YellowBar")) {
                        tempBoundary2P = x + 1;
                    } else if (BarColorClassifier.DetectBarStateSF5(color) == "noize" && x < temp2P) {
                        gradientCount2P++;
                    } else if (gradient > thresholdSF5 && BarColorClassifier.DetectBarStateSF5(color) == "NoHealth" && x < temp2P) {
                        gradientCount2P++;
                    }
                    prevIntensity = intensity;
                }
                if (Math.Abs(minHPBoundary2P * 3 + maxHPBoundary2P) / 4 > temp2P && tempBoundary2P > Math.Abs(minHPBoundary2P + maxHPBoundary2P * 3) / 4) {
                    tempBoundary2P = minHPBoundary2P;
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
                // Note: boundaries[2] and [3] are swapped — 2P bar's inner edge comes before outer in sorted order
                maxHPBoundary2P = boundaries[3];
                minHPBoundary2P = boundaries[2];
                tempBoundary1P = maxHPBoundary1P;
                OnMessage?.Invoke($"基準フレームの境界 1P: {maxHPBoundary1P} - {minHPBoundary1P}, 2P: {minHPBoundary2P} - {maxHPBoundary2P}");
            } else {
                OnMessage?.Invoke("境界が正しく検出されていません。");
            }
        }

        public void SaveHPPercentagesToCSV(string outputPath, AnalysisResult result) {
            using (StreamWriter sw = new StreamWriter(outputPath)) {
                sw.WriteLine("FrameNumber,LeftHP[%],RightHP[%],ErrorInformation");
                for (int i = 0; i < result.HP1P.Count; i++) {
                    sw.WriteLine($"{i},{result.HP1P[i]},{result.HP2P[i]},{result.Errors[i]}");
                }
            }
            OnMessage?.Invoke("体力割合をCSVに保存しました。");
        }
    }
}
