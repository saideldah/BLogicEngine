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
    abstract public class LineOfBusinessRepository : NonEntityRepository<LineOfBusinessEntity, Guid>
    {
        protected override string TableName { get { return "LineOfBusiness"; } }
        public static LineOfBusinessRepository CreateInstance()
        {
            LineOfBusinessRepository lineOfBusinessRepository = RepositoryFactory.Create<LineOfBusinessRepository>("ProductSetupConnection");
            return lineOfBusinessRepository;
        }

        public LineOfBusinessEntity Insert(LineOfBusinessEntity lineOfBusinessEntityObject)
        {
            lineOfBusinessEntityObject.Id = Guid.NewGuid();
            base.DB.Execute("usp_LineOfBusiness_Insert", lineOfBusinessEntityObject);
            return lineOfBusinessEntityObject;
        }

        public List<LineOfBusinessEntity> InsertMany(List<LineOfBusinessEntity> lineOfBusinessEntityObjectList)
        {
            foreach (var lineOfBusinessEntityObject in lineOfBusinessEntityObjectList)
            {
                lineOfBusinessEntityObject.Id = Guid.NewGuid();
            }
            base.DB.Execute("usp_LineOfBusiness_InsertMany", lineOfBusinessEntityObjectList);

            return lineOfBusinessEntityObjectList;
        }


        public void LinkLineOfBusinessToProperty(LineOfBusinessEntity lineOfBusinessEntityObject, PropertyEntity propertyEntity)
        {
            var lobProperty = new { Id = Guid.NewGuid(), Value = propertyEntity.Value , PropertyId  = propertyEntity.Id , LineOfBusinessId = lineOfBusinessEntityObject .Id};
            base.DB.Execute("usp_LineOfBusinessProperty_Insert", lobProperty);
            
        }

        public void LinkLineOfBusinessToManyProperty(LineOfBusinessEntity lineOfBusinessEntityObject, List<PropertyEntity> propertyEntityList)
        {
            List<object> lobPropertyList = new List<object>();
            foreach (var propertyEntity in propertyEntityList)
            {
                lobPropertyList.Add(new { Id = Guid.NewGuid(), Value = propertyEntity.Value, PropertyId = propertyEntity.Id, LineOfBusinessId = lineOfBusinessEntityObject.Id });
            }

            base.DB.Execute("usp_LineOfBusinessProperty_InsertMany", lobPropertyList);

        }

    }
}
