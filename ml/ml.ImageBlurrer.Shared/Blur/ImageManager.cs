using System.Drawing;

namespace ml.ImageBlurrer.Shared
{
    /// <summary>
    /// Алгоритмы для blur'а
    /// </summary>
    public class ImageManager
    {
        public void GetImageAndSaveBlurred(string imageName, string editedImageName, string imageDirectory, BlurDescription blurDescription = null)
        {
            var imagePath = $"{imageDirectory}{imageName}";
            var editedImagePath = $"{imageDirectory}{editedImageName}";

            var image = new Bitmap(imagePath);
            
            if (blurDescription == null)
            {
                blurDescription = new BlurDescription(image);
            }
            
            var editedImage = GetBlurredImageOf(image, blurDescription);

            editedImage.Save(editedImagePath);
        }

        public Bitmap GetImage(string imageName, string imageDirectory)
        {
            var imagePath = $"{imageDirectory}{imageName}";
            return new Bitmap(imagePath);
        }
        
        public Bitmap GetBlurredImageOf(Bitmap initialImage, BlurDescription description)
        {
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