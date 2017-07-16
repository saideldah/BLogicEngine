using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insight.Database;
using BSynchro.Web.Entities;
using System.Data;


namespace Challenges.DataAccess
{
    abstract public class ParticipantRepository : NonEntityRepository<ParticipantEntity, Guid>
    {
        protected override string TableName { get { return "ChallengeField"; } }
        public static ParticipantRepository CreateInstance()
        {
            ParticipantRepository participantRepository = RepositoryFactory.Create<ParticipantRepository>("ChallengesAppDB");
            return participantRepository;
        }
        public override IList<ParticipantEntity> SelectAll()
        {
            return base.DB.Query<ParticipantEntity>("usp_Participant_SelectAll");
        }

        public IList<ParticipantEntity> SelectByAgentCode(string agentCode)
        {
            return base.DB.Query<ParticipantEntity>("usp_Participant_SelectByAgentCode");
        }
        public IList<ParticipantEntity> SelectByAgencyCode(string agencyCode)
        {
            return base.DB.Query<ParticipantEntity>("usp_Participant_SelectByAgencyCode");
        }

        public IList<ParticipantEntity> SelectByRegionalCode(string regionalCode)
        {
            return base.DB.Query<ParticipantEntity>("usp_Participant_SelectByRegionalCode");
        }

        public IList<ParticipantEntity> SelectByChallengeId(string challengeId)
        {
            return base.DB.Query<ParticipantEntity>("usp_Participant_SelectByChallengeId", new { ChallengeId = challengeId });
        }
        public override Guid Insert(ParticipantEntity participantEntity)
        {
            participantEntity.Id = Guid.NewGuid();
            participantEntity = base.DB.Query<ParticipantEntity>("usp_Participant_Insert", participantEntity).FirstOrDefault();
            return participantEntity.Id;
        }

        public List<ParticipantEntity> InsertMany(List<ParticipantEntity> participantEntityList)
        {
            foreach (var item in participantEntityList)
            {
                item.Id = this.Insert(item);
            }
            return participantEntityList;
        }
    }
}
