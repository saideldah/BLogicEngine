using JIC.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Business.ProductSetup.Model
{
    public class LineOfBusinessModel : BaseModel
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public List<PackageModel> PackageList { get; set; }
        public List<PropertyModel> PropertyList { get; set; }

        public LineOfBusinessModel()
        {
            PackageList = new List<PackageModel>();
            PropertyList = new List<PropertyModel>();
        }
    }
}
