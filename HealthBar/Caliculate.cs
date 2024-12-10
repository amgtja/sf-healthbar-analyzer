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
    }
}
