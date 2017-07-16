using System;
using System.Collections.Generic;
using System.Linq;
using Insight.Database;
using BSynchro.Web.Entities;

namespace LovManager.DataAccess
{
    abstract public class DomainRepository : NonEntityRepository<DomainEntity, Guid>
    {
        protected override string TableName { get { return "Domain"; } }
        public static DomainRepository CreateInstance()
        {
            DomainRepository domainRepository = RepositoryFactory.Create<DomainRepository>("LovManagerDatabase");
            domainRepository.DB.Open();
            return domainRepository;
        }

        public override Guid Insert(DomainEntity domainEntity)
        {
            domainEntity.Id = Guid.NewGuid();
            base.DB.Execute("usp_Domain_Insert", domainEntity);

            return domainEntity.Id;
        }
        public void Update(DomainEntity domainEntity)
        {
            base.DB.Execute("usp_Domain_Update", domainEntity);
        }
        public void Delete(DomainEntity domainEntity)
        {
            base.DB.Execute("usp_Domain_Delete", domainEntity);
        }
        public override IList<DomainEntity> SelectAll()
        {
            return base.DB.Query<DomainEntity>("usp_Domain_SelectAll");
        }

        public IList<DomainEntity> SelectByName(string name)
        {
            return base.DB.Query<DomainEntity>("usp_Domain_SelectByName", new { Name = name });
        }
        public DomainEntity SelectByCode(string code)
        {
            return base.DB.Query<DomainEntity>("usp_Domain_SelectByCode", new { Code = code }).FirstOrDefault();
        }
        public DomainEntity SelectById(string Id)
        {
            return base.DB.Query<DomainEntity>("usp_Domain_SelectById", new { Id = Id }).FirstOrDefault();
        }
    }
}
