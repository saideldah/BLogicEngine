using JIC.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Business.ProductSetup.Model
{
    public class LineOfBusinessGroupModel : BaseModel
    {

        public string Code { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// contain all Line LineOfBusiness
        /// </summary>
        public List<LineOfBusinessModel> LineOfBusinessList { get; set; }
        public LineOfBusinessGroupModel()
        {
            LineOfBusinessList = new List<LineOfBusinessModel>();
        }

    }
}
