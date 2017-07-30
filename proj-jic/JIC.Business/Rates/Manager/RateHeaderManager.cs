using JIC.Business.Rates.Model;
using JIC.DataAccess.Rates.Entity;
using JIC.DataAccess.Rates.Repository;
using System;
using System.Collections.Generic;
namespace JIC.Business.Rates.Manager
{
    public class RateHeaderManager
    {
        #region Private Fields
        private readonly List<string> genders;
        private readonly Lazy<RateHeaderRepository> rateRepository;
        #endregion

        #region Constructor
        public RateHeaderManager()
        {
            genders = new List<string> { "M", "F" };
            rateRepository = new Lazy<RateHeaderRepository>(GetRateHeaderRepository);
        }
        #endregion

        #region Private Methods
        private RateHeaderRepository GetRateHeaderRepository()
        {
            return RateHeaderRepository.CreateInstance();
        }
        private RateEntity RateModelToRateEntity(string productCode, string benefitCode, RateModel rate)
        {
            RateEntity rateEntity = new RateEntity();
            if (string.IsNullOrWhiteSpace(productCode) || string.IsNullOrWhiteSpace(benefitCode) || rate == null)
            {
                return rateEntity;
            }
            rateEntity.PRCF_PROD = productCode;
            rateEntity.PRCF_COVER_P = benefitCode;
            rateEntity.PRCF_RAT_MIN = rate.Value;
            rateEntity.PRCF_AGE = rate.Age;
            int date;
#warning Validate Date Format
            int.TryParse(rate.StartDate.ToString("yyyyddMM"), out date);
            rateEntity.PRCF_DATE = date;
            return rateEntity;
        }
        #endregion

        #region Public Methods
        public int Upsert(FooterRateRequestModel request)
        {
            var rowsEffected = 0;
#warning The below should be configurable later on
            if (request == null) return 0;
            request.ProductCode = "171";
            if (request.Rates == null) return 0;
            foreach (var rate in request.Rates)
            {
                foreach (var gender in genders)
                {
                    var entity = rateRepository.Value.FindSingle(new { request.ProductCode, request.BenefitCode, rate.Age, Gender = gender });
                    if (entity == null)//Insert a new rate entity
                    {
                        if (gender == "F")
                        {
                            rate.Age = rate.Age + 3;
                            entity = RateModelToRateEntity(request.ProductCode, request.BenefitCode, rate);
                            rowsEffected += rateRepository.Value.Insert(entity);
                        }
                        else
                        {
                            if (rate.Age == 18)
                            {
                                for (int count = 0; count <= 3; count++)
                                {
                                    entity = RateModelToRateEntity(request.ProductCode, request.BenefitCode, rate);
                                    rate.Age = rate.Age + 1;
                                    rowsEffected += rateRepository.Value.Insert(entity);
                                }
                            }
                        }
                    }
                    else //update the existing
                    {
                        if (gender == "F")
                        {
                            rate.Age = rate.Age + 3;
                            entity = RateModelToRateEntity(request.ProductCode, request.BenefitCode, rate);
                            rowsEffected += rateRepository.Value.Update(entity);
                        }
                        else
                        {
                            if (rate.Age == 18)
                            {
                                for (int count = 0; count < 3; count++)
                                {
                                    rate.Age = rate.Age + 1;
                                    entity = RateModelToRateEntity(request.ProductCode, request.BenefitCode, rate);
                                    rowsEffected += rateRepository.Value.Update(entity);
                                }

                            }
                        }

                    }
                }
            }
            return rowsEffected;
        }
        #endregion
    }
}