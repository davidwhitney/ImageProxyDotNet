using System.Drawing.Imaging;

namespace ImageProxy.Net.Code
{
    public class SourcedImage
    {
        public byte[] Bytes { get; set; }
        public ImageFormat Format { get; set; }

        public SourcedImage(byte[] bytes, ImageFormat format)
        {
            Bytes = bytes;
            Format = format;
        }
    }
}