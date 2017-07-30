using JIC.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Business.ProductSetup.Model
{
    public class ProductMappingModel : BaseModel
    {

        public string MappingRule { get; set; }

        public string CodeBack { get; set; }
        public string Status { get; set; }

    }
}
