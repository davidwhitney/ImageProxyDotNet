using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImageProxy.Net.Code.ImageManipulation 
{
    public static class BitmapUtilities 
    {
        public static BitmapFrame ReadBitmapFrame(MemoryStream photoStream) 
        {
            var photoDecoder = BitmapDecoder.Create(
                photoStream,
                BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.None);
            return photoDecoder.Frames[0];
        }

        public static byte[] ToByteArray(this BitmapFrame targetFrame, ImageFormat format)
        {
            if(format == ImageFormat.Jpeg)
            {
                return ToByteArray<JpegBitmapEncoder>(targetFrame);
            }
            if(format == ImageFormat.Png)
            {
                return ToByteArray<PngBitmapEncoder>(targetFrame); 
            }
            if(format == ImageFormat.Bmp)
            {
                return ToByteArray<BmpBitmapEncoder>(targetFrame); 
            }
            if(format == ImageFormat.Tiff)
            {
                return ToByteArray<TiffBitmapEncoder>(targetFrame); 
            }
            if(format == ImageFormat.Wmf)
            {
                return ToByteArray<WmpBitmapEncoder>(targetFrame); 
            }
            if(format == ImageFormat.Gif)
            {
                return ToByteArray<GifBitmapEncoder>(targetFrame); 
            }

            throw new InvalidOperationException("Image format not supported");
        }

        public static byte[] ToByteArray<TEncoder>(this BitmapFrame targetFrame) where TEncoder: BitmapEncoder, new()
        {
            byte[] targetBytes;
            using (var memoryStream = new MemoryStream()) {
                var targetEncoder = new TEncoder();
                targetEncoder.Frames.Add(targetFrame);
                targetEncoder.Save(memoryStream);
                targetBytes = memoryStream.ToArray();
            }
            return targetBytes;
        }
    }
}