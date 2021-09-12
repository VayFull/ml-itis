using System.Drawing;
using System.IO;

namespace ml.ImageBlurrer
{
    public static class ImageExtension
    {
        public static string GetImageDirectoryPath() => $@"{Directory.GetCurrentDirectory()}\Images\";
        
        public static Bitmap GetImageFromPath(string path) => new Bitmap(path);
    }
}