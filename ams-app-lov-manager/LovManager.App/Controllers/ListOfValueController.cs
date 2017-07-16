using LovManager.Api.Helper;
using LovManager.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace LovManager.Api.Controllers
{
    public class ListOfValueController : ApiController
    {
        private DomainManager domainManager = new DomainManager();
        private ListOfValueManager listOfValueManager = new ListOfValueManager();

        [HttpGet]
        [ActionName("GetFile")]
        public HttpResponseMessage GetFile(string domainId)
        {
            var listOfValueModelList = new List<ListOfValueModel>();

            listOfValueModelList = listOfValueManager.GetListOfValueByDomainId(domainId);
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(listOfValueModelList.ToCsv(true));
            writer.Flush();
            stream.Position = 0;

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "Export.csv" };
            return result;
        }


        [HttpGet]
        [ActionName("GetTemplate")]
        public HttpResponseMessage GetTemplate()
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
