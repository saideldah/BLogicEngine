using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Business.Model
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
