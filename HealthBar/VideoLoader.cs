using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace HealthBar {
    public class VideoLoader {
        public VideoCapture capture;
        public HPBarForm form;
        public int TotalFrames { get; set; }
        public int currentframe = -1;
        public Bitmap originalBitmap;
        public Bitmap scaledBitmap;
        public Mat reusableFrame = new Mat();
        public VideoLoader(HPBarForm form) {
            this.form = form;
        }

        //動画ファイルを読み込むメソッド

        public bool LoadVideo(string filePath) {
            capture = new VideoCapture(filePath);

            //動画ファイルの読み込み確認
            if (!capture.IsOpened()) { return false; } else {
                TotalFrames = (int)capture.FrameCount;
                currentframe = -1;
                return true;
            }
        }

        public Bitmap GetFrameRead(int framenumber) {

            //例外処理
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
        public void GetFrameMat(int framenumber) {

            //例外処理
            if (capture == null) {
                throw new InvalidOperationException("動画がロードされていません");
            }
            Mat frame = new Mat();
            if (currentframe + 1 == framenumber) {
                currentframe = framenumber;
                try {
                    capture.Read(frame);
                    reusableFrame = frame;
                } catch (Exception e) {
                    Console.WriteLine(e);
                }
            } else {
                currentframe = framenumber;
                capture.PosFrames = currentframe;
                try {
                    capture.Read(frame);
                    reusableFrame = frame;
                } catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
            frame.Dispose();
        }
        public Bitmap ScaledDisplay(Bitmap frame) {
            if (frame == null) return null;
            originalBitmap = frame;
            scaledBitmap = new Bitmap(originalBitmap, form.pictureBoxFrame.Width, 720);
            return scaledBitmap;
        }

    }
}
