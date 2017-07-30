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
    abstract public class CoverRepository : NonEntityRepository<CoverEntity, Guid>
    {
        protected override string TableName { get { return "Cover"; } }
        public static CoverRepository CreateInstance()
        {
            CoverRepository coverRepository = RepositoryFactory.Create<CoverRepository>("ProductSetupConnection");
            return coverRepository;
        }
        public CoverEntity Insert(CoverEntity coverEntityObject)
        {
            coverEntityObject.Id = Guid.NewGuid();
            base.DB.Execute("usp_Cover_Insert", coverEntityObject);
            return coverEntityObject;
        }

        public List<CoverEntity> InsertMany(List<CoverEntity> coverEntityObjectList)
        {
            foreach (var coverEntityObject in coverEntityObjectList)
            {
                coverEntityObject.Id = Guid.NewGuid();
            }
            base.DB.Execute("usp_Cover_InsertMany", coverEntityObjectList);

            return coverEntityObjectList;
        }

        public void LinkCoverToPackage(CoverEntity coverEntityObject, PackageEntity packageEntityObject)
        {
            var CoverPackage = new { Id = Guid.NewGuid(), CoverId = coverEntityObject.Id, PackageId = packageEntityObject.Id };
            base.DB.Execute("usp_CoverPackage_Insert", CoverPackage);

        }

        public void LinkManyCoverToPackage(List<CoverEntity> coverEntityObjectList, PackageEntity packageEntity)
        {
            List<object> CoverPackageList = new List<object>();
            foreach (var CoverEntityObject in coverEntityObjectList)
            {
                CoverPackageList.Add(new { Id = Guid.NewGuid(), CoverId = CoverEntityObject.Id, PackageId = packageEntity.Id });
            }

            base.DB.Execute("usp_CoverPackage_InsertMany", CoverPackageList);

        }
    }
}
