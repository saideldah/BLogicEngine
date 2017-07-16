using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovManager.DataAccess
{
    public class DomainEntity
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public Guid ParentDomainId { get; set; }
    }
}
