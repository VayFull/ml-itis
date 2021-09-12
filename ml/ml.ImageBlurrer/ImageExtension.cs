using System.Drawing;
using System.IO;

namespace ml.ImageBlurrer
{
    public static class ImageExtension
    {
        public static string GetImageDirectoryPath() => $@"{Directory.GetCurrentDirectory()}\Images\";
        
        public static Bitmap GetImageFromPath(string path) => new(path);

        public static Bitmap GetBlurredImage(Bitmap initialImage)
        {
            var description = new BlurDescription(initialImage);

            for (int xFragment = 0; xFragment < description.FragmentCount; xFragment++)
            {
                for (int yFragment = 0; yFragment < description.FragmentCount; yFragment++)
                {
                    var xStartBlurPosition =
                        description.StartBlurPosition.X + description.BlurFragmentSize.Width * xFragment;
                    
                    var xFinishBlurPosition = xStartBlurPosition + description.BlurFragmentSize.Width;
                    
                    var yStartBlurPosition =
                        description.StartBlurPosition.Y + description.BlurFragmentSize.Height * yFragment;

                    var yFinishBlurPosition = yStartBlurPosition + description.BlurFragmentSize.Height;

                    var r = 0;
                    var g = 0;
                    var b = 0;

                    for (int xBlurPosition = xStartBlurPosition; xBlurPosition < xFinishBlurPosition; xBlurPosition++)
                    {
                        for (int yBlurPosition = yStartBlurPosition; yBlurPosition < yFinishBlurPosition; yBlurPosition++)
                        {
                            var color = initialImage.GetPixel(xBlurPosition, yBlurPosition);
                            r += color.R;
                            g += color.G;
                            b += color.B;
                        }
                    }

                    var total = (xFinishBlurPosition - xStartBlurPosition) * (yFinishBlurPosition - yStartBlurPosition);
                    
                    r /= total;
                    g /= total;
                    b /= total;

                    var averageColor = Color.FromArgb(r, g, b);
                    
                    for (int xBlurPosition = xStartBlurPosition; xBlurPosition < xFinishBlurPosition; xBlurPosition++)
                    {
                        for (int yBlurPosition = yStartBlurPosition; yBlurPosition < yFinishBlurPosition; yBlurPosition++)
                        {
                            initialImage.SetPixel(xBlurPosition, yBlurPosition, averageColor);
                        }
                    }
                }
            }

            return initialImage;
        }
    }
}