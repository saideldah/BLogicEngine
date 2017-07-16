using System;
using Insight.Database;
using BSynchro.Web.Entities;
using LovManager.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace LovManager.DataAccess
{
    abstract public class ListOfValueRepository : NonEntityRepository<ListOfValueEntity, Guid>
    {
        protected override string TableName { get { return "ListOfValue"; } }
        public static ListOfValueRepository CreateInstance()
        {
            ListOfValueRepository listOfValueRipository = RepositoryFactory.Create<ListOfValueRepository>("LovManagerDatabase");
            return listOfValueRipository;
        }

        public override Guid Insert(ListOfValueEntity listOfValueEntity)
        {
            listOfValueEntity.Id = Guid.NewGuid();
            base.DB.Execute("usp_ListOfValue_Insert", listOfValueEntity);

            return listOfValueEntity.Id;
        }
        public void Update(ListOfValueEntity listOfValueEntity)
        {
            base.DB.Execute("usp_ListOfValue_Update", listOfValueEntity);
        }

        public override IList<ListOfValueEntity> SelectAll()
        {
            return base.DB.Query<ListOfValueEntity>("usp_ListOfValue_SelectAll");
        }

        public IList<ListOfValueEntity> SelectByName(string name)
        {
            return base.DB.Query<ListOfValueEntity>("usp_Domain_SelectByName", new { Name = name });
        }

        public IList<ListOfValueEntity> SelectByDomainId(string domainId)
        {
            return base.DB.Query<ListOfValueEntity>("usp_ListOfValue_SelectByDomainId", new { DomainId = domainId });
        }
        public ListOfValueEntity SelectByCode(string code)
        {
            return base.DB.Query<ListOfValueEntity>("usp_ListOfValue_SelectByCode", new { Code = code }).FirstOrDefault();
        }
        public ListOfValueEntity SelectById(string Id)
        {
            return base.DB.Query<ListOfValueEntity>("usp_ListOfValue_SelectById", new { Id = Id }).FirstOrDefault();
        }
    }
}
