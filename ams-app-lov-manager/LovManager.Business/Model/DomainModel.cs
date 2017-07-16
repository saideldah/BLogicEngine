using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovManager.Business
{
    public class DomainModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public DomainModel ParentDomain { get; set; }
        public List<ListOfValueModel> ListOfValueList { get; set; }
        public DomainModel()
        {
            ListOfValueList = new List<ListOfValueModel>();
        }
    }
}
