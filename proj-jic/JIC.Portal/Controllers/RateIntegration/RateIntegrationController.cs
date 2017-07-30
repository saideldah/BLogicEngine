using BSynchro.Web.AMSEvents;
using JIC.Business.Rates.Manager;
using JIC.Business.Rates.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace JIC.Portal.Controllers
{
    public class RateIntegrationController : ApiController
    {
        #region Private Properties
        private readonly Lazy<RateHeaderManager> manager;
        #endregion

        #region Constructor
        public RateIntegrationController()
        {
            manager = new Lazy<RateHeaderManager>(GetRateHeaderManager);
        }
        #endregion

        #region Private Methods
        private RateHeaderManager GetRateHeaderManager()
        {
            return new RateHeaderManager();
        }
        #endregion

        #region Public Methods

        [HttpPost]
        public HttpResponseMessage Post(FooterRateRequestModel rateRequest)
        {
            try
            {
                var rateResponse = new HttpResponseMessage();

                #region Business Logic Validation
                bool isValid = true;
                List<ErrorModel> errors = new List<ErrorModel>();
                if(!ModelState.IsValid)
                {
                    var error = new ErrorModel() { Type = "Validation", Message = "Invalid parameter value", Code = "001" };
                    errors.Add(error);
                }
                if (rateRequest == null || rateRequest.Rates == null)
                {
                    var error = new ErrorModel() { Type = "Validation", Message = "Request Model Null", Code = "" };
                    errors.Add(error);
                }
                if (errors.Count > 0)
                {
                    isValid = false;
                }
                #endregion

                if (!isValid)
                {
                    var response = new ResponseModel() { Status = HttpStatusCode.BadRequest.ToString(), Message = "Found some errors, please check error section" };
                    response.Errors = errors;
                    rateResponse = Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }
                else
                {
                    if (manager.Value.Upsert(rateRequest) > 0)
                    {
                        rateResponse = Request.CreateResponse(HttpStatusCode.OK, new ResponseModel() { Status = HttpStatusCode.OK.ToString(), Message = "Success", Data = rateRequest });
                    }
                    else
                    {
                        var error = new ErrorModel() { Type = "Validation", Message = "No records found.", Code = "002" };
                        errors.Add(error);
                        var response = new ResponseModel() { Status = HttpStatusCode.NotFound.ToString(), Message = "No records found.", Data = rateRequest, Errors = errors};
                        rateResponse = Request.CreateResponse(HttpStatusCode.NotFound, response);
                    }
                }
                return rateResponse;
            }
            catch (Exception exception)
            {
                exception.Handle<RateIntegrationController>();
                var error = new ErrorModel() { Type = exception.GetType().ToString(), Message = exception.Message };
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, new ResponseModel() { Status = "Error", Message = exception.Message, Data = error });
                return response;
            }
        }
        #endregion



    }
}
