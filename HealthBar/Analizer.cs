using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace HealthBar {
    public class Analizer {
        public VideoLoader videoL;

        public void AnalizeAndExport(string csvFilePath) {
            using (StreamWriter writer = new StreamWriter(csvFilePath)) {

                //csvファイルのヘッダー
                writer.WriteLine("Frame,LeftHP,RightHP,Error");

                //動画の全フレーム解析
                for (int i = 0; i < videoL.TotalFrames; i++) {
                    Bitmap frame = videoL.GetFrameAt(i);
                    if (frame != null) {
                        //体力ゲージの解析
                        int LeftHP = AnalyzeHPBar(frame, new Rectangle(50, 25, 100, 10));
                        int RightHP = AnalyzeHPBar(frame, new Rectangle(490, 25, 100, 10));
                    }
                }
            }
        }
        public int AnalyzeHPBar(Bitmap frame, Rectangle barArea) {

            //HPBarの領域を決定
            Bitmap barBitmap = frame.Clone(barArea, frame.PixelFormat);

            //色、ピクセルの解析
            Mat barMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(barBitmap);

            //グレースケール画像に変換して解析
            Mat grayMat = new Mat();
            Cv2.CvtColor(barMat, grayMat, ColorConversionCodes.BGR2GRAY);

            //ゲージの輝度平均算出
            Scalar meanBright = Cv2.Mean(grayMat);

            //ゲージを輝度で計算
            int maxBright = 255;
            int HPPercent = (int)(meanBright.Val0/maxBright*100);

            return HPPercent;
        }
    }
}
