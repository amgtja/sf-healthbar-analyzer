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
        public int TotalFrames { get; private set; }
        public int thresh=130;

        //動画ファイルを読み込むメソッド

        public bool LoadVideo(string filePath) {
            capture = new VideoCapture(filePath);

            //動画ファイルの読み込み確認
            if (!capture.IsOpened()) { return false; } else {
                TotalFrames = (int)capture.FrameCount;
                return true;
            }
        }

        //特定のフレームを取得する
        public Bitmap GetFrameAt(int frameIndex) {
            if (capture == null) {
                throw new InvalidOperationException("動画がロードされていません");
            }
            int totalFrames = (int)capture.FrameCount;
            if (frameIndex < 0 || frameIndex >= totalFrames) {
                throw new ArgumentOutOfRangeException(nameof(frameIndex));
            }

            //指定されたフレームにシーク
            capture.Set(VideoCaptureProperties.PosFrames, frameIndex);

            //フレーム取得
            Mat frame = new Mat();
            if (capture.Read(frame) && !frame.Empty()) {
                return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame);
            } else { return null; }
        }
        public Bitmap ToBW(int frameIndex) {
            //画像をグレースケールで出力させてみる
            Mat frameTemp = OpenCvSharp.Extensions.BitmapConverter.ToMat(GetFrameAt(frameIndex));
            Mat grayMat = new Mat();
            Cv2.CvtColor(frameTemp, grayMat, ColorConversionCodes.BGR2GRAY);
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(grayMat);
            //画像を二値化（白黒に）
            Mat bwMat = new Mat();
            Cv2.Threshold(grayMat, bwMat, thresh, 255, ThresholdTypes.Binary);
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(bwMat);
            OpenCvSharp.Point[][] contours;
            OpenCvSharp.HierarchyIndex[] hindex;
            Cv2.FindContours(bwMat, out contours, out hindex,RetrievalModes.List,ContourApproximationModes.ApproxNone);
            Cv2.DrawContours(bwMat, contours, -1, new Scalar(255, 0, 0), 2);
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(bwMat);

        }

    }
}
