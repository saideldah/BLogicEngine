using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LovManager.Api.Helper
{
    public class ResponseObject
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public object Data  { get; set; }

    }
}