using System.Web.Mvc;

namespace ImageProxy.Net.Code.ActionResults
{
    public class RawBytesActionResult : System.Web.Mvc.ActionResult
    {
        private readonly byte[] _bytes;
        private readonly string _contentType;

        public RawBytesActionResult(byte[] bytes, string contentType)
        {
            _bytes = bytes;
            _contentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = _contentType;

            response.OutputStream.Write(_bytes, 0, _bytes.Length);
        }
    }
}