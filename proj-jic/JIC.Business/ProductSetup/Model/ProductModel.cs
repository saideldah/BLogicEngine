using JIC.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Business.ProductSetup.Model
{
    public class ProductModel : BaseModel
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public List<ProductMappingModel> ProductMappingList { get; set; }
        public List<CoverModel> CoverList { get; set; }

        public ProductModel()
        {
            ProductMappingList = new List<ProductMappingModel>();
            CoverList = new List<CoverModel>();
        }
    }
}
