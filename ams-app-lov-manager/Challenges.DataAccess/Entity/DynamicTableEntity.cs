using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenges.DataAccess
{
    public class DynamicTableEntity : ParticipantEntity
    {
        public Dictionary<string, double> DynamicFields { get; set; }

        public DynamicTableEntity()
        {
            this.DynamicFields = new Dictionary<string, double>();
        }
    }
}
