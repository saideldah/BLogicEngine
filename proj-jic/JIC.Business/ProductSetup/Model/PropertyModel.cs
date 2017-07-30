using JIC.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Business.ProductSetup.Model
{
    public class PropertyModel : BaseModel
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }

        public string DataType { get; set; }
        public string PropertyType { get; set; }

        public PropertyModel()
        {

        }
        public PropertyModel(PropertyModel propertyModel)
        {
            this.Code = propertyModel.Code;
            this.Title = propertyModel.Title;
            this.Value = propertyModel.Value;
            this.DataType = propertyModel.DataType;
            this.PropertyType = propertyModel.PropertyType;
        }

    }
}
