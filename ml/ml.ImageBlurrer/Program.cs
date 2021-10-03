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

            var superPixelManager = new SuperPixelManager();
            const string editedSuperPixelImageName = "editedSuperPixelImageName.jpg";
            superPixelManager.GetImageAndSaveSuperPixelized(imageName, editedSuperPixelImageName, imageDirectory, 0.1f, 40);

            

            /*for (float i = 0.1f; i < 2f; i+= 0.35f)
            {
                superPixelManager.GetImageAndSaveSuperPixelized(imageName, $"k={i.ToString()}c=12{editedSuperPixelImageName}", 
                    imageDirectory, i, 12);
            }*/

            //var image = imageManager.GetImage(imageName, imageDirectory);
        }
    }
}
