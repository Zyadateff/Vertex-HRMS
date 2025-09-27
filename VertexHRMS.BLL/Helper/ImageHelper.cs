using System.Drawing;
using System.Drawing.Imaging;

namespace VertexHRMS.BLL.Helper
{
    public static class ImageHelper
    {
        public static void ResizeImage(string inputPath, string outputPath, int maxSize = 400)
        {
            using var image = System.Drawing.Image.FromFile(inputPath);
            int width, height;

            if (image.Width > image.Height)
            {
                width = maxSize;
                height = (int)((double)image.Height / image.Width * maxSize);
            }
            else
            {
                height = maxSize;
                width = (int)((double)image.Width / image.Height * maxSize);
            }

            using var bitmap = new Bitmap(image, new Size(width, height));
            bitmap.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
