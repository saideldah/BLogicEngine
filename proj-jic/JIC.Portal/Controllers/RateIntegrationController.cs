using JIC.Business.Manager;
using JIC.Business.Models.Rate;
using System;
using System.Web.Http;
namespace JIC.Portal.Controllers
{
    public class RateIntegrationController : ApiController
    {
        [HttpPost]
        public RateResponse Save(BenefitRequestRateHeader rateRequest)
        {
            try
            {
                RateResponse rateResponse = new RateResponse();
                if (rateRequest == null)
                {
                    rateResponse.SetErrors("The Request is Null");
                    rateResponse.Status = false;
                    return rateResponse;
                }
                else
                {
                    if (rateRequest != null)
                    {
                        RateHeaderManager rateManager = new RateHeaderManager();
                        rateRequest.ProductCode = rateManager.InsertHeaderRate(rateRequest).ToString();
                        if (rateRequest.ProductCode != null)
                        {
                            rateResponse.Status = true;
                            return rateResponse;
                        }
                        else
                        {
                            rateResponse.Status = false;
                            return rateResponse;
                        }
                    }
                }
                return rateResponse;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //[HttpPost]
        //public RateResponse Update(BenefitRequestRateHeader rateRequest)
        //{
        //    try
        //    {
        //        RateResponse rateResponse = new RateResponse();
        //        if (rateRequest == null)
        //        {
        //            rateResponse.SetErrors(ErrorMessage.NO_RECORD_FOUND_MESSAGE, ErrorCode.NO_RECORD_FOUND_CODE);
        //            rateResponse.Status = false;
        //            return rateResponse;
        //        }
        //        else
        //        {
        //            if (rateRequest != null)
        //            {
        //                RateHeaderManager rateManager = new RateHeaderManager();
        //                rateRequest.ProductCode = rateManager.UpdateRate(rateRequest).ToString();
        //                if (rateRequest.ProductCode != null)
        //                {
        //                    rateResponse.Status = true;
        //                    return rateResponse;
        //                }
        //                else
        //                {
        //                    rateResponse.Status = false;
        //                    return rateResponse;
        //                }
        //            }
        //        }
        //        return rateResponse;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}
    }
}
