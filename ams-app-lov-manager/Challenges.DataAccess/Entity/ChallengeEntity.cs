using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenges.DataAccess
{
    public class ChallengeEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Status { get; set; }
        public string RelatedTableName { get; set; }
        public string Columns { get; set; }
        public string DisplayedColumns { get; set; }

        public string ExcelUrl { get; set; }
        public DateTime LastModificationDate { get; set; }
        public IEnumerable<ParticipantEntity> Participants { get; set; }
    }
}
