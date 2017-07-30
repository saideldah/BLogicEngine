using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Business
{
    public abstract class ErrorType
    {
        public static string ValidationError
        {
            get
            {
                return "ValidationError";
            }
        }
    }
}
