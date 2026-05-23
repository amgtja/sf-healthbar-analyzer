using System;
using System.Drawing;
using OpenCvSharp;

namespace HealthBar {
    public class VideoLoader : IFrameReader {
        private VideoCapture capture;
        private readonly Func<int> getDisplayWidth;
        public int TotalFrames { get; private set; }
        public int currentframe = -1;

        public VideoLoader(Func<int> getDisplayWidth) {
            this.getDisplayWidth = getDisplayWidth;
        }

        public bool LoadVideo(string filePath) {
            capture = new VideoCapture(filePath);
            if (!capture.IsOpened()) { return false; }
            TotalFrames = (int)capture.FrameCount;
            currentframe = -1;
            return true;
        }

        public Bitmap GetFrameRead(int framenumber) {
            if (capture == null) {
                throw new InvalidOperationException("動画がロードされていません");
            }
            using (Mat frame = new Mat()) {
                if (currentframe + 1 == framenumber) {
                    currentframe = framenumber;
                    if (capture.Read(frame) && !frame.Empty()) {
                        return ScaledDisplay(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame));
                    } else { return null; }
                } else {
                    currentframe = framenumber;
                    capture.PosFrames = currentframe;
                    if (capture.Read(frame) && !frame.Empty()) {
                        return ScaledDisplay(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame));
                    } else { return null; }
                }
            }
        }

        private Bitmap ScaledDisplay(Bitmap source) {
            if (source == null) return null;
            int targetWidth = getDisplayWidth();
            int targetHeight = (int)(source.Height * ((double)targetWidth / source.Width));
            var scaled = new Bitmap(source, targetWidth, targetHeight);
            source.Dispose();
            return scaled;
        }
    }
}
