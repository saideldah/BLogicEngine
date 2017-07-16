using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LovManager.Api.Helper;
using LovManager.Business;
using System.Net.Http.Headers;

namespace LovManager.Api.Controllers
{
    public class TemplateController : ApiController
    {
        [HttpGet]
        [ActionName("Get")]
        public HttpResponseMessage Get()
        {
            var lovObjectList = new List<ListOfValueModel>();
            //header

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(lovObjectList.ToCsv(true));
            writer.Flush();
            stream.Position = 0;

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "Export.csv" };
            return result;
        }
    }
}
