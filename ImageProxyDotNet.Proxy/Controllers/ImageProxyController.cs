using System.Drawing.Imaging;
using System.Web;
using System.Web.Mvc;
using ImageProxy.Net.Code;
using ImageProxy.Net.Code.ActionResults;
using ImageProxy.Net.Code.ImageManipulation;
using ImageProxy.Net.Code.PhysicalImageProviders;

namespace ImageProxy.Net.Controllers
{
    public class ImageProxyController : Controller
    {
        private readonly ImageSourcingService _imageSourcingService;

        public ImageProxyController():this(new ImageSourcingService(new WebServerPhysicalImageProvider()))
        {
        }

        private ImageProxyController(ImageSourcingService imageSourcingService)
        {
            _imageSourcingService = imageSourcingService;
        }

        [HttpGet]
        [HandleError]
        [OutputCache(VaryByParam = "category,name,width,height,format", Duration = 5)]
        public ActionResult RenderImage(string category, string name, double? width, double? height, ImageFormat format = null)
        {
            var sourcedImage = _imageSourcingService.SourceImage(category, name, width, height, format);
            return Image(sourcedImage);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            throw new HttpException(404, "File not found.");
        }

        public ActionResult Image(SourcedImage image)
        {
            return new RawBytesActionResult(image.Bytes, image.Format.ContentType());
        }
    }
}
