using BSynchro.Web.Entities;
using Insight.Database;
using JIC.DataAccess.Rates.Entity;
namespace JIC.DataAccess.Rates.Repository
{
    public abstract class RateHeaderRepository : NonEntityRepository<RateEntity, int>
    {
        #region Protected Fields
        protected override string TableName { get { return "PRCF"; } }
        #endregion

        #region Public Static Methods
        public static RateHeaderRepository CreateInstance()
        {
            return RepositoryFactory.Create<RateHeaderRepository>("RateIntegrationDB");
        }
        #endregion

        #region Public Methods

        public override int Insert(RateEntity rateEntity)
        {            
            return DB.Execute("usp_Rate_Insert", rateEntity);
        }

        public int Update(RateEntity rateEntity)
        {
            return DB.Execute("usp_Rate_Update", rateEntity);
        }
        #endregion
    }
}
