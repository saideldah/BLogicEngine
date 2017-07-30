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
    abstract public class CoverMappingRepository : NonEntityRepository<CoverMappingEntity, Guid>
    {
        protected override string TableName { get { return "CoverMapping"; } }
        public static CoverMappingRepository CreateInstance()
        {
            CoverMappingRepository CoverMappingRepository = RepositoryFactory.Create<CoverMappingRepository>("ProductSetupConnection");
            return CoverMappingRepository;
        }
        public CoverMappingEntity Insert(CoverMappingEntity CoverMappingEntityObject)
        {
            CoverMappingEntityObject.Id = Guid.NewGuid();
            base.DB.Execute("usp_CoverMapping_Insert", CoverMappingEntityObject);
            return CoverMappingEntityObject;
        }

        public List<CoverMappingEntity> InsertMany(List<CoverMappingEntity> CoverMappingEntityObjectList)
        {
            foreach (var CoverMappingEntityObject in CoverMappingEntityObjectList)
            {
                CoverMappingEntityObject.Id = Guid.NewGuid();
            }
            base.DB.Execute("usp_CoverMapping_InsertMany", CoverMappingEntityObjectList);

            return CoverMappingEntityObjectList;
        }

        public void LinkCoverMappingToPackage(CoverMappingEntity CoverMappingEntityObject, PackageEntity packageEntityObject)
        {
            var CoverMappingPackage = new { Id = Guid.NewGuid(), CoverMappingId = CoverMappingEntityObject.Id, PackageId = packageEntityObject.Id };
            base.DB.Execute("usp_CoverMappingPackage_Insert", CoverMappingPackage);

        }

        public void LinkManyCoverMappingToPackage(List<CoverMappingEntity> CoverMappingEntityObjectList, PackageEntity packageEntity)
        {
            List<object> CoverMappingPackageList = new List<object>();
            foreach (var CoverMappingEntityObject in CoverMappingEntityObjectList)
            {
                CoverMappingPackageList.Add(new { Id = Guid.NewGuid(), CoverMappingId = CoverMappingEntityObject.Id, PackageId = packageEntity.Id });
            }

            base.DB.Execute("usp_CoverMappingPackage_InsertMany", CoverMappingPackageList);

        }
    }
}
