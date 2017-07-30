using JIC.Business.Models.Rate;
using JIC.DataAccess.Entity;
using JIC.DataAccess.Repositories;
using System;
namespace JIC.Business.Manager
{
    public class RateHeaderManager
    {
        #region Private

        #region Fields
        private RateHeaderRepository rateRepository;
        #endregion

        #region Mapper
        private RateHeaderEntity EntityToFooterMapper(BenefitRequestRateHeader rateRequest)
        {
            RateHeaderEntity rateResult = new RateHeaderEntity()
            {
                PRCF_PROD = rateRequest.ProductCode.ToString(),
                PRCF_COVER_P = rateRequest.BenefitCode.ToString(),
                PRCF_RAT_MIN = rateRequest.Rates[0].Value,
                PRCF_AGE = rateRequest.Rates[0].Age,
                PRCF_DATE = Convert.ToInt32(rateRequest.Rates[0].StartDate.ToString("yyyyddMM"))
            };
            return rateResult;
        }

        #endregion

        #endregion

        #region public

        #region Ctors

        public RateHeaderManager()
        {
            rateRepository = RateHeaderRepository.CreateInstance();
        }

        #endregion

        #region Public Method

        public RateHeaderEntity InsertHeaderRate(BenefitRequestRateHeader rate)
        {
            RateHeaderEntity rateEntity = EntityToFooterMapper(rate);
            rateEntity.PRCF_PROD= rateRepository.SelectRate(rateEntity);
                if (rateEntity.PRCF_PROD == "171")
                    rateEntity.PRCF_PROD = rateRepository.InsertRate(rateEntity);
                    return rateEntity;
        }

        //public string UpdateRate(BenefitRequestRateHeader rate)
        //{

        //    RateHeaderEntity rateEntity = EntityToFooterMapper(rate);

        //            rateEntity.PRCF_PROD = rateRepository.UpdateRate(rateEntity);
        //            return rateEntity.PRCF_PROD;
        //}


        //public string UpdateRate(BenefitRequestRateHeader rate)
        //{

        //    RateHeaderEntity rateEntity = EntityToFooterMapper(rate);
        //    rateEntity.PRCF_SEX = rateRepository.SelectGender(rateEntity);
        //    rateEntity.PRCF_PROD = rateRepository.SelectProductCode(rateEntity);
        //    if (rateEntity.PRCF_PROD == "171")
        //    {
        //        if (rateEntity.PRCF_SEX == "M")
        //        {
        //            rateEntity.PRCF_PROD = rateRepository.UpdateMaleRate(rateEntity);
        //        }
        //        else 
        //        {
        //            rateEntity.PRCF_PROD = rateRepository.UpdateFemaleRate(rateEntity);
        //        }
        //    }
        //    return rateEntity.PRCF_PROD;
        //}

        #endregion

        #endregion
    }
}