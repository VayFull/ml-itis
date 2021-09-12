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
            
            imageManager.GetImageAndSaveBlurred(imageName, editedImageName, imageDirectory);
        }
    }
}
