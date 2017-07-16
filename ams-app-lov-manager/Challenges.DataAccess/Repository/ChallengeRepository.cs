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
    abstract public class ChallengeRepository : NonEntityRepository<ChallengeEntity, Guid>
    {
        protected override string TableName { get { return "Challenge"; } }
        public static ChallengeRepository CreateInstance()
        {
            ChallengeRepository challengeRepository = RepositoryFactory.Create<ChallengeRepository>("ChallengesAppDB");
            return challengeRepository;
        }
        public override IList<ChallengeEntity> SelectAll()
        {
            return base.DB.Query<ChallengeEntity>("usp_Challenge_SelectAll");
        }
        public IList<ChallengeEntity> SelectByPartcipantId(Guid participantId)
        {
            return base.DB.Query<ChallengeEntity>("usp_Challenge_SelectByPartcipantId", new { ParticipantId = participantId });
        }
        public IList<ChallengeEntity> SelectByAgentCode(string agentCode)
        {
            return base.DB.Query<ChallengeEntity>("usp_Challenge_SelectByAgentCode", new { AgentCode = agentCode });
        }
        public IList<ChallengeEntity> SelectByAgencyCode(string agencyCode)
        {
            return base.DB.Query<ChallengeEntity>("usp_Challenge_SelectByAgencyCode", new { AgencyCode = agencyCode });
        }
        public IList<ChallengeEntity> SelectByRegionalCode(string regionalCode)
        {
            return base.DB.Query<ChallengeEntity>("usp_Challenge_SelectByRegionalCode", new { RegionalCode = regionalCode });
        }
        public ChallengeEntity Select(string Id)
        {
            return base.DB.Query<ChallengeEntity>("usp_Challenge_Select", new { Id = Id }).FirstOrDefault();
        }
        public override Guid Insert(ChallengeEntity challengeEntity)
        {
            challengeEntity.Id = Guid.NewGuid();
            base.DB.Execute("usp_Challenge_Insert", challengeEntity);

            return challengeEntity.Id;
        }
        public void Update(ChallengeEntity challengeEntity)
        {
            base.DB.Execute("usp_Challenge_Update", challengeEntity);
        }
        public void LinkChallengeToParticipant(ChallengeEntity challengeEntity, ParticipantEntity participantEntity) 
        {
            base.DB.Execute("usp_ChallengeParticipant_Insert", new { ChallengeId = challengeEntity.Id.ToString(), ParticipantId = participantEntity.Id.ToString() });
        }
        public void DeleteLinkedParticipants(ChallengeEntity challengeEntity)
        {
            base.DB.Execute("usp_ChallengeParticipant_Delete", new { ChallengeId = challengeEntity.Id.ToString()});
        }
        public void LinkChallengeToParticipantList(ChallengeEntity challengeEntity, List<ParticipantEntity> participantEntityList)
        {
            foreach (var participant in participantEntityList)
            {
                this.LinkChallengeToParticipant(challengeEntity, participant);
            }
        }
    }
}
