using System.Drawing;

namespace ml.ImageBlurrer.Shared
{
    public class BlurDescription
    {
        public Point StartBlurPosition { get; set; }
        public Size BlurSize { get; set; }
        public Size BlurFragmentSize { get; set; }
        public int FragmentCount { get; set; }
        
        public BlurDescription(Bitmap initialImage)
        {
            var width = initialImage.Width;
            var height = initialImage.Height;

            FragmentCount = 20;

            StartBlurPosition = new Point(width / 4, height / 4);
            BlurSize = new Size(width / 2, height / 2);
            BlurFragmentSize = new Size(BlurSize.Width / FragmentCount, BlurSize.Height / FragmentCount);
        }
    }
}