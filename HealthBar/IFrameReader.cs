using System.Drawing;

namespace HealthBar {
    public interface IFrameReader {
        Bitmap GetFrameRead(int frameNumber);
        int TotalFrames { get; }
    }
}
