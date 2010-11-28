using System.Drawing.Imaging;

namespace ImageProxy.Net.Code.ImageManipulation
{
    public static class ImageFormatExtensions
    {
        public static string ContentType (this ImageFormat format)
        {
            return "image/" + format.ToString().ToLower();
        }
    }
}