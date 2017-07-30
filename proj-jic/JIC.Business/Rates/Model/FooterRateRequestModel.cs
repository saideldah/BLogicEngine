using System.Collections.Generic;
namespace JIC.Business.Rates.Model
{
    public class FooterRateRequestModel : BaseRateModel
    {
        public string BenefitCode { get; set; }
        public List<RateModel> Rates { get; set; }

    }
}
