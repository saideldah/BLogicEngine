using System.Collections;
using System.Collections.Generic;

namespace JIC.Business.Rates.Model
{
    public class ResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public List<ErrorModel> Errors { get; set; }
        public ResponseModel()
        {
            Errors = new List<ErrorModel>();
        }
    }
}
