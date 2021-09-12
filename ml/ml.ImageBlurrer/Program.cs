namespace ml.ImageBlurrer
{
    class Program
    {
        static void Main(string[] args)
        {
            const string imageName = "image.jpg";
            const string editedImageName = "editedImageName.jpg";
            
            var imageDirectory = ImageExtension.GetImageDirectoryPath();
            
            var imagePath = $"{imageDirectory}{imageName}";
            var editedImagePath = $"{imageDirectory}{editedImageName}";

            var image = ImageExtension.GetImageFromPath(imagePath);
            var editedImage = ImageExtension.GetBlurredImage(image);

            editedImage.Save(editedImagePath);
        }
    }
}
