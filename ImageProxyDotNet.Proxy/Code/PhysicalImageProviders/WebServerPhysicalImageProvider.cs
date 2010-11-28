using System.Web;

namespace ImageProxy.Net.Code.PhysicalImageProviders
{
    public class WebServerPhysicalImageProvider : IPhysicalImageProvider
    {
        public byte[] LoadImage(string category, string name)
        {
            var photoPath = GenerateImagePath(category, name);
            return System.IO.File.ReadAllBytes(photoPath);
        }

        private static string GenerateImagePath(string category, string name)
        {
            return HttpContext.Current.Server.MapPath(string.Format("/App_Data/{0}/{1}.jpg", category, name));
        }
    }
}