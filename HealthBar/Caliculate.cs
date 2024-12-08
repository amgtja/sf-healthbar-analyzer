using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthBar {
    public class Caliculate {
        public HPBarForm form;

        public Caliculate(HPBarForm form) {
            this.form = form;
        }

        public List<byte> GetBright(int currentFrame, int y) {
            List<byte> brightValue = new List<byte>();
            Bitmap frameBitmap = form.videoL.GetFrameRead(currentFrame);

            int width = frameBitmap.Width;
            Mat frame = OpenCvSharp.Extensions.BitmapConverter.ToMat(form.videoL.GetFrameRead(currentFrame));
            for (int x = 0; x < width; x++) {
                byte brightness = frame.At<byte>(y, x);
                brightValue.Add(brightness);
            }
            frame.Dispose();
            frameBitmap.Dispose();
            return brightValue;
        }
        public (List<byte> R, List<byte> G, List<byte> B) GetRGB(int currentFrame, int y) {
            List<byte> rValues = new List<byte>();
            List<byte> gValues = new List<byte>();
            List<byte> bValues = new List<byte>();

            Bitmap frameBitmap = form.videoL.GetFrameRead(currentFrame);
            Mat frame = OpenCvSharp.Extensions.BitmapConverter.ToMat(frameBitmap);

            for (int x = 0; x < frame.Width; x++) {
                Vec3b color = frame.At<Vec3b>(y, x); // OpenCVのVec3b型でRGB値を取得
                bValues.Add(color.Item0); // B成分
                gValues.Add(color.Item1); // G成分
                rValues.Add(color.Item2); // R成分
            }
            frame.Dispose();
            frameBitmap.Dispose();
            return (rValues, gValues, bValues);
        }

        public List<int> Gradient1(int currentFrame, int y) {
            List<int> gradients = new List<int>();
            Bitmap frameBitmap = form.videoL.GetFrameRead(currentFrame);
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

            frame.Dispose();
            frameBitmap.Dispose();
            //gradient[X座標]で0に1のX座標が入っている、RGBの総和を出す
            return gradients;
        }
        public List<int> GradientNarrow(int currentFrame, int y, int HPLeft, int HPRight) {
            List<int> gradients = new List<int>();

            Bitmap frameBitmap = form.videoL.GetFrameRead(currentFrame);
            Mat frame = OpenCvSharp.Extensions.BitmapConverter.ToMat(frameBitmap);

            // RGB値の総和を計算し、輝度の代わり
            List<int> intensityValues = new List<int>();

            for (int x = HPLeft-1; x <= HPRight; x++) {
                Vec3b color = frame.At<Vec3b>(y, x);
                int intensity = color.Item0 + color.Item1 + color.Item2; // R + G + Bの総和を計算
                intensityValues.Add(intensity);
            }

            // 勾配（一次微分）の計算
            for (int i = 1; i < intensityValues.Count; i++) {
                int gradient = Math.Abs(intensityValues[i] - intensityValues[i - 1]);
                gradients.Add(gradient);
            }

            frame.Dispose();
            frameBitmap.Dispose();
            //gradient[X座標]で0に1のX座標が入っている、RGBの総和を出す
            return gradients;
        }
    }
}
