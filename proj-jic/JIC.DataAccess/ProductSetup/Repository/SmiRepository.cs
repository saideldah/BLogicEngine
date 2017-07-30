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
    abstract public class SmiRepository : NonEntityRepository<SmiEntity, Guid>
    {
        protected override string TableName { get { return "Smi"; } }
        public static SmiRepository CreateInstance()
        {
            SmiRepository SmiRepository = RepositoryFactory.Create<SmiRepository>("ProductSetupConnection");
            return SmiRepository;
        }
        public SmiEntity Insert(SmiEntity SmiEntityObject)
        {
            SmiEntityObject.Id = Guid.NewGuid();
            base.DB.Execute("usp_Smi_Insert", SmiEntityObject);
            return SmiEntityObject;
        }

        public List<SmiEntity> InsertMany(List<SmiEntity> SmiEntityObjectList)
        {
            foreach (var SmiEntityObject in SmiEntityObjectList)
            {
                SmiEntityObject.Id = Guid.NewGuid();
            }
            base.DB.Execute("usp_Smi_InsertMany", SmiEntityObjectList);

            return SmiEntityObjectList;
        }

        public void LinkSmiToProperty(SmiEntity SmiEntityObject, PropertyEntity propertyEntity)
        {
            var SmiPackage = new { Id = Guid.NewGuid(), SmiId = SmiEntityObject.Id, PropertyId = propertyEntity.Id, Value = propertyEntity.Value };
            base.DB.Execute("usp_SmiProperty_Insert", SmiPackage);

        }

    }
}
