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
                var photo = BitmapUtilities.ReadBitmapFrame(photoStream);

                if (photo == null)
                {
                    throw new HttpException(404, "File not found.");
                }

                if (RequestIsForNativeImageSize(photo, width, height))
                {
                    return new SourcedImage(photo.ToByteArray(format), format);
                }

                var scaleX = (width.GetValueOrDefault(photo.Width) / photo.Width);
                var scaleY = (height.GetValueOrDefault(photo.Height) / photo.Height);

                var target = new TransformedBitmap(photo, new ScaleTransform(scaleX, scaleY, 0, 0));

                var targetFrame = BitmapFrame.Create(target);
                var targetBytes = targetFrame.ToByteArray(format);


                return new SourcedImage(targetBytes, format);
            }
        }

        private static bool RequestIsForNativeImageSize(ImageSource photo, double? width, double? height)
        {
            return width.GetValueOrDefault(photo.Width) == photo.Width
                   && height.GetValueOrDefault(photo.Height) == photo.Height;
        }
    }
}