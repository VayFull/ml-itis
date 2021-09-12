﻿using System.Drawing;

namespace ml.ImageBlurrer.Shared
{
    public class ImageManager
    {
        public void GetImageAndSaveBlurred(string imageName, string editedImageName, string imageDirectory)
        {
            var imagePath = $"{imageDirectory}{imageName}";
            var editedImagePath = $"{imageDirectory}{editedImageName}";

            var image = new Bitmap(imagePath);
            var editedImage = GetBlurredImageOf(image);

            editedImage.Save(editedImagePath);
        }
        
        public Bitmap GetBlurredImageOf(Bitmap initialImage)
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