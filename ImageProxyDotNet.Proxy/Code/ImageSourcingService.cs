using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageProxy.Net.Code.ImageManipulation;
using ImageProxy.Net.Code.PhysicalImageProviders;

namespace ImageProxy.Net.Code
{
    public class ImageSourcingService
    {
        private readonly IPhysicalImageProvider _imageProvider;

        public ImageSourcingService():this(new WebServerPhysicalImageProvider())
        {
        }

        public ImageSourcingService(IPhysicalImageProvider imageProvider)
        {
            _imageProvider = imageProvider;
        }

        public SourcedImage SourceImage(string category, string name, double? width, double? height, ImageFormat format = null)
        {
            if (format == null)
            {
                format = ImageFormat.Jpeg;
            }

            var photoBytes = _imageProvider.LoadImage(category, name);

            using (var photoStream = new MemoryStream(photoBytes))
            {
                var photo = ReadBitmap(photoStream);

                return RequestIsForNativeImageSize(photo, width, height) 
                    ? new SourcedImage(photo.ToByteArray(format), format) 
                    : new SourcedImage(ResizeImage(format, photo, width, height), format);
            }
        }

        private static byte[] ResizeImage(ImageFormat format, BitmapSource photo, double? width, double? height)
        {
            var scaleX = (width.GetValueOrDefault(photo.Width) / photo.Width);
            var scaleY = (height.GetValueOrDefault(photo.Height) / photo.Height);

            var target = new TransformedBitmap(photo, new ScaleTransform(scaleX, scaleY, 0, 0));

            var targetFrame = BitmapFrame.Create(target);
            return targetFrame.ToByteArray(format);
        }

        private static bool RequestIsForNativeImageSize(ImageSource photo, double? width, double? height)
        {
            return width.GetValueOrDefault(photo.Width) == photo.Width
                   && height.GetValueOrDefault(photo.Height) == photo.Height;
        }

        public static BitmapFrame ReadBitmap(MemoryStream photoStream)
        {
            var frame = BitmapDecoder.Create(photoStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None).Frames[0];
            
            if(frame == null)
            {
                throw new HttpException(404, "File not found.");
            }

            return frame;
        }
    }
}