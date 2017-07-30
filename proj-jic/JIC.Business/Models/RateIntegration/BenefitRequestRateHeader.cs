using System.Collections.Generic;
namespace JIC.Business.Models.RateIntegration
{
    public class BenefitRequestRateHeader : CommonRate
    {      
       public string BenefitCode { get; set; }
       public List<Rate> Rates { get; set; }
    }
}
