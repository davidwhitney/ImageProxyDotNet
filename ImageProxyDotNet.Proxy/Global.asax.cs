using System.Drawing.Imaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ImageProxy.Net
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "DefaultJpeg",
                "{category}/{name}.jpg", 
                new { controller = "ImageProxy", action = "RenderImage", width = UrlParameter.Optional , height = UrlParameter.Optional, format = ImageFormat.Jpeg }
            );
            
            routes.MapRoute(
                "DefaultWithFormat",
                "{category}/{name}.{format}", 
                new { controller = "ImageProxy", action = "RenderImage", width = UrlParameter.Optional , height = UrlParameter.Optional }
            ); 
         
            routes.MapRoute(
                "Default",
                "{category}/{name}", 
                new { controller = "ImageProxy", action = "RenderImage", width = UrlParameter.Optional , height = UrlParameter.Optional, format = ImageFormat.Jpeg }
            );
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}