using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Business.ProductSetup.Model
{
    public class ProductSetupModel
    {
        /// <summary>
        /// contain all Line LineOfBusinessGroup 
        /// </summary>
        public List<LineOfBusinessGroupModel> LineOfBusinessGroupList { get; set; }
        public List<SmiModel> SmiList { get; set; }

        /// <summary>
        /// this list contain all Property from (Package Parameters, Package Parameter Definition, SMI, Packages) without any redundancy 
        /// </summary>
        public List<PropertyModel> PropertyList { get; set; }
        public ProductSetupModel()
        {
            LineOfBusinessGroupList = new List<LineOfBusinessGroupModel>();
            SmiList = new List<SmiModel>();
            PropertyList = new List<PropertyModel>();
        }

    }
}
