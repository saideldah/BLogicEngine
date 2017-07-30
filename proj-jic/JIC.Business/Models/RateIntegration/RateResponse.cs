using JIC.Business.Validator;
namespace JIC.Business.Models.RateIntegration
{
    public class RateResponse
    {
        public RateError[] Errors { get; set; }
        public bool Status { get; set; }

        public void SetErrors(RateError error)
        {
            this.Errors = new RateError[] { error };
        }
        public void SetErrors(string message, string code)
        {
            this.Errors = new RateError[] {
                new RateError(message, code)
            };
        }

        public void SetErrors(string message)
        {
            this.Errors = new RateError[] {
                new RateError(message)
            };
        }


    }
}


    
