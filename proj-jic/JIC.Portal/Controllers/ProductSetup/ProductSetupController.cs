using JIC.Business.Model;
using JIC.Business.ProductSetup.Manager;
using JIC.Business.ProductSetup.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JIC.Portal.Controllers.ProductSetup
{
    public class ProductSetupController : ApiController
    {
        private ProductSetupManager productSetupManager;
        public ProductSetupController()
        {
            this.productSetupManager = new ProductSetupManager();
        }
        [HttpPost]
        [ActionName("Validate")]
        public HttpResponseMessage Validate(ProductSetupFileModel productSetupFileModel)
        {
            try
            {
                var response = new HttpResponseMessage();
                List<ErrorModel> errorList = productSetupManager.Validate(productSetupFileModel);
                if (errorList.Count > 0)
                {
                    response = Request.CreateResponse(
                        HttpStatusCode.BadRequest,
                        new ResponseModel()
                        {
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = "Please Check Errors Section",
                            Errors = errorList
                        });
                }
                else
                {
                    response = Request.CreateResponse(
                        HttpStatusCode.OK, new ResponseModel()
                        {
                            Status = HttpStatusCode.OK.ToString(),
                            Message = "",
                            Data = "done"
                        });

                }
                return response;
            }
            catch (Exception e)
            {

                var response = new HttpResponseMessage();
                var error = new ErrorModel() { Type = e.GetType().ToString(), Message = e.Message };
                List<ErrorModel> errorList = new List<ErrorModel>();
                errorList.Add(error);
                response = Request.CreateResponse(
                     HttpStatusCode.InternalServerError,
                     new ResponseModel()
                     {
                         Status = HttpStatusCode.InternalServerError.ToString(),
                         Message = "Please Check Errors Section",
                         Errors = errorList
                     });
                return response;
            }
        }


        [HttpPost]
        [ActionName("Post")]
        public HttpResponseMessage Post(ProductSetupFileModel productSetupFileModel)
        {
            try
            {
                var response = new HttpResponseMessage();
                List<ErrorModel> errorList = productSetupManager.SaveFile(productSetupFileModel);
                if (errorList.Count > 0)
                {
                    response = Request.CreateResponse(
                        HttpStatusCode.BadRequest,
                        new ResponseModel()
                        {
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = "Please Check Errors Section",
                            Errors = errorList
                        });
                }
                else
                {
                    response = Request.CreateResponse(
                        HttpStatusCode.OK, new ResponseModel()
                        {
                            Status = HttpStatusCode.OK.ToString(),
                            Message = "",
                            Data = "done"
                        });

                }
                return response;
            }
            catch (Exception e)
            {

                var response = new HttpResponseMessage();
                var error = new ErrorModel() { Type = e.GetType().ToString(), Message = e.Message };
                List<ErrorModel> errorList = new List<ErrorModel>();
                errorList.Add(error);
                response = Request.CreateResponse(
                     HttpStatusCode.InternalServerError,
                     new ResponseModel()
                     {
                         Status = HttpStatusCode.InternalServerError.ToString(),
                         Message = "Please Check Errors Section",
                         Errors = errorList
                     });
                return response;
            }
        }


        [HttpPost]
        [ActionName("Rollback")]
        public HttpResponseMessage Rollback(ProductSetupFileModel productSetupFileModel)
        {
            try
            {
                var response = new HttpResponseMessage();
                List<ErrorModel> errorList = productSetupManager.Rollback(productSetupFileModel.Version);
                if (errorList.Count > 0)
                {
                    response = Request.CreateResponse(
                        HttpStatusCode.BadRequest,
                        new ResponseModel()
                        {
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = "Please Check Errors Section",
                            Errors = errorList
                        });
                }
                else
                {
                    response = Request.CreateResponse(
                        HttpStatusCode.OK, new ResponseModel()
                        {
                            Status = HttpStatusCode.OK.ToString(),
                            Message = "",
                            Data = "done"
                        });

                }
                return response;
            }
            catch (Exception e)
            {

                var response = new HttpResponseMessage();
                var error = new ErrorModel() { Type = e.GetType().ToString(), Message = e.Message };
                List<ErrorModel> errorList = new List<ErrorModel>();
                errorList.Add(error);
                response = Request.CreateResponse(
                     HttpStatusCode.InternalServerError,
                     new ResponseModel()
                     {
                         Status = HttpStatusCode.InternalServerError.ToString(),
                         Message = "Please Check Errors Section",
                         Errors = errorList
                     });
                return response;
            }
        }

    }
}
