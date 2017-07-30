using JIC.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Business.ProductSetup.Model
{
    public class CoverModel : BaseModel
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string CodeBack { get; set; }

        public List<PropertyModel> PropertyList { get; set; }
        public CoverModel()
        {
            PropertyList = new List<PropertyModel>();
        }
    }
}
