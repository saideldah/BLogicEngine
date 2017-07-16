using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using LovManager.Api.Helper;
using System.Net.Http.Headers;
using LovManager.Business;
using System;
using BSynchro;


namespace LovManager.Api.Controllers
{

    public class DomainController : ApiController
    {

        private DomainManager domainManager = new DomainManager();
        private ListOfValueManager listOfValueManager = new ListOfValueManager();



        [HttpPost]
        [ActionName("Post")]
        public HttpResponseMessage Post()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                string name = httpRequest.Form["name"];
                string parentDomainId = httpRequest.Form["parentDomainId"];

                DomainModel domainModel = new DomainModel();
                domainModel.Code = name.ToInternalName();
                domainModel.Name = name;
                domainModel.ParentDomain = new DomainModel() { Id = parentDomainId };
                

                var response = new HttpResponseMessage();
                if (httpRequest.Files.Count == 0)
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, new ResponseObject() { Status = HttpStatusCode.BadRequest.ToString(), Message = "No File Uploaded" });
                    return response;
                }

                var postedFile = httpRequest.Files[0];

                //Validate File
                bool valid = listOfValueManager.ValidateListOfValueFileStream(domainModel, postedFile.InputStream);

                if (valid)
                {
                    //Save domain
                    domainModel = domainManager.Save(domainModel);
                    //Save Lov List
                    listOfValueManager.Save(domainModel, postedFile.InputStream);
                }
                
                response = Request.CreateResponse(HttpStatusCode.OK, new ResponseObject() { Status = "OK", Message = "", Data = "done" });
                return response;
            }
            catch (Exception e)
            {

                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, new ResponseObject() { Status = "Error", Message = e.Message });
                return response;
            }
            
        }


        [HttpPost]
        [ActionName("Put")]
        public HttpResponseMessage Put()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                string domainName = httpRequest.Form["domainName"];
                string domainCode = httpRequest.Form["domainCode"];
                string domainId = httpRequest.Form["domainId"];

                string parentDomainId = httpRequest.Form["parentDomainId"];

                DomainModel domainModel = new DomainModel();
                domainModel.Code = domainCode;
                domainModel.Name = domainName;
                domainModel.Id = domainId;

                domainModel.ParentDomain = new DomainModel() { Id = parentDomainId };


                var response = new HttpResponseMessage();
                if (httpRequest.Files.Count == 0)
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, new ResponseObject() { Status = HttpStatusCode.BadRequest.ToString(), Message = "No File Uploaded" });
                    return response;
                }

                var postedFile = httpRequest.Files[0];

                //Validate File
                bool valid = listOfValueManager.ValidateListOfValueFileStream(domainModel, postedFile.InputStream);

                if (valid)
                {
                    //Update Lov List
                    listOfValueManager.Update(domainModel, postedFile.InputStream);
                }

                response = Request.CreateResponse(HttpStatusCode.OK, new ResponseObject() { Status = "OK", Message = "", Data = "done" });
                return response;
            }
            catch (Exception e)
            {

                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, new ResponseObject() { Status = "Error", Message = e.Message });
                return response;
            }

        }

        [HttpGet]
        [ActionName("Get")]
        public HttpResponseMessage Get(string id)
        {
            DomainModel domain = domainManager.GetById(id);
            var response = Request.CreateResponse(HttpStatusCode.OK, new ResponseObject() { Status = "OK", Message = "", Data = domain });
            return response;
        }

        [HttpGet]
        [ActionName("Get")]
        public HttpResponseMessage Get()
        {
            var domainList = domainManager.GetAll();
            var response = Request.CreateResponse(HttpStatusCode.OK, new ResponseObject() { Status = "OK", Message = "", Data = domainList });
            return response;
        }

    }
}
