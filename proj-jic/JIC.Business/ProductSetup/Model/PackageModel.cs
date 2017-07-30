using JIC.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Business.ProductSetup.Model
{
    public class PackageModel : BaseModel
    {

        public string Code { get; set; }
        public string Title { get; set; }
        public string SmiWizardType { get; set; }
        public string SavingsCalculator { get; set; }
        public string Status { get; set; }

        public List<ProductModel> ProductList { get; set; }
        public List<PropertyModel> PropertyList { get; set; }
        public PackageModel()
        {
            ProductList = new List<ProductModel>();
            PropertyList = new List<PropertyModel>();
        }
    }
}
