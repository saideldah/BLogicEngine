namespace JIC.Business.Validator
{
    public class RateError
    {

        public string ErroCode { get; set; }

        public string ErrorMessage { get; set; }
        public RateError()
        {
        }

        public RateError(string message, string code = null)
        {
            this.ErrorMessage = message;
            this.ErroCode = code;
        }
    }
}
