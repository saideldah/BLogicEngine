using JIC.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Business.ProductSetup.Model
{
    public class SmiModel : BaseModel
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public bool RetrievedFromBank { get; set; }
        public string FieldControlOnScreen { get; set; }
        public string Description { get; set; }

        public List<PropertyModel> PropertyList { get; set; }
        public SmiModel()
        {
            PropertyList = new List<PropertyModel>();
        }
    }
}
