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
    abstract public class LineOfBusinessGroupRepository : NonEntityRepository<LineOfBusinessGroupEntity, Guid>
    {
        protected override string TableName { get { return "LOBGroup"; } }
        public static LineOfBusinessGroupRepository CreateInstance()
        {
            LineOfBusinessGroupRepository lineOfBusinessGroupRepository = RepositoryFactory.Create<LineOfBusinessGroupRepository>("ProductSetupConnection");
            return lineOfBusinessGroupRepository;
        }


        public LineOfBusinessGroupEntity Insert(LineOfBusinessGroupEntity lineOfBusinessGroupEntityObject)
        {
            lineOfBusinessGroupEntityObject.Id = Guid.NewGuid();
            base.DB.Execute("usp_LobGroup_Insert", lineOfBusinessGroupEntityObject);
            return lineOfBusinessGroupEntityObject;
        }

        public List<LineOfBusinessGroupEntity> InsertMany(List<LineOfBusinessGroupEntity> lineOfBusinessGroupEntityObjectList)
        {
            foreach (var lineOfBusinessGroupEntityObject in lineOfBusinessGroupEntityObjectList)
            {
                lineOfBusinessGroupEntityObject.Id = Guid.NewGuid();
            }
            base.DB.Execute("usp_LobGroup_InsertMany", lineOfBusinessGroupEntityObjectList);

            return lineOfBusinessGroupEntityObjectList;
        }
    }
}
