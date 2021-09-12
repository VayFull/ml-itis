using ml.ImageBlurrer.Shared;

namespace ml.ImageBlurrer
{
    class Program
    {
        static void Main(string[] args)
        {
            var imageManager = new ImageManager();
            const string imageName = "image.jpg";
            const string editedImageName = "editedImageName.jpg";
            
            var imageDirectory = ImageExtension.GetImageDirectoryPath();
            
            var imagePath = $"{imageDirectory}{imageName}";
            var editedImagePath = $"{imageDirectory}{editedImageName}";

            var image = ImageExtension.GetImageFromPath(imagePath);
            var editedImage = imageManager.GetBlurredImageOf(image);

            editedImage.Save(editedImagePath);
        }
    }
}
