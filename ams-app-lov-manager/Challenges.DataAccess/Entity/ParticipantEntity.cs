using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenges.DataAccess
{
    public class ParticipantEntity
    {
        public Guid Id { get; set; }
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public string AgencyCode { get; set; }
        public string IntermediaryCode { get; set; }
        public string RegionalCode { get; set; }
        public double CancellationPoint { get; set; }
    }
}
