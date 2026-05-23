using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace HealthBar {
    public class Calculate {
        private readonly IFrameReader frameReader;

        public Calculate(IFrameReader frameReader) {
            this.frameReader = frameReader;
        }

        public List<int> Gradient1(int currentFrame, int y) {
            List<int> gradients = new List<int>();
            Bitmap frameBitmap = frameReader.GetFrameRead(currentFrame);
            Mat frame = OpenCvSharp.Extensions.BitmapConverter.ToMat(frameBitmap);

            List<int> intensityValues = new List<int>();
            for (int x = 0; x < frame.Width; x++) {
                Vec3b color = frame.At<Vec3b>(y, x);
                int intensity = color.Item0 + color.Item1 + color.Item2;
                intensityValues.Add(intensity);
            }

            for (int i = 1; i < intensityValues.Count; i++) {
                int gradient = Math.Abs(intensityValues[i] - intensityValues[i - 1]);
                gradients.Add(gradient);
            }

            frame.Dispose();
            frameBitmap.Dispose();
            return gradients;
        }
    }
}
