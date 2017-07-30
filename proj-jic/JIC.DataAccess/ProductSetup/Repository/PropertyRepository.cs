using BSynchro.Web.Entities;
using Insight.Database;
using JIC.DataAccess.ProductSetup.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.DataAccess.ProductSetup.Repository
{
    abstract public class PropertyRepository : NonEntityRepository<PropertyEntity, Guid>
    {

        protected override string TableName { get { return "Property"; } }
        public static PropertyRepository CreateInstance()
        {
            PropertyRepository domainRepository = RepositoryFactory.Create<PropertyRepository>("ProductSetupConnection");
            return domainRepository;
        }
        public PropertyEntity Insert(PropertyEntity domainEntity)
        {
            domainEntity.Id = Guid.NewGuid();
            base.DB.Execute("usp_Property_Insert", domainEntity);

            return domainEntity;
        }
        public List<PropertyEntity> InsertMany(List<PropertyEntity> propertyEntityObjectList)
        {
            foreach (var propertyEntityObject in propertyEntityObjectList)
            {
                propertyEntityObject.Id = Guid.NewGuid();
            }
            base.DB.Execute("usp_Property_InsertMany", propertyEntityObjectList);

            return propertyEntityObjectList;
        }
    }
}
