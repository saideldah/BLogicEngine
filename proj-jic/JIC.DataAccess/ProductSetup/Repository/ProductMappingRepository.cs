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
    abstract public class ProductMappingRepository : NonEntityRepository<ProductMappingEntity, Guid>
    {
        protected override string TableName { get { return "ProductMapping"; } }
        public static ProductMappingRepository CreateInstance()
        {
            ProductMappingRepository ProductMappingRepository = RepositoryFactory.Create<ProductMappingRepository>("ProductSetupConnection");
            return ProductMappingRepository;
        }
        public ProductMappingEntity Insert(ProductMappingEntity ProductMappingEntityObject)
        {
            ProductMappingEntityObject.Id = Guid.NewGuid();
            base.DB.Execute("usp_ProductMapping_Insert", ProductMappingEntityObject);
            return ProductMappingEntityObject;
        }

        public List<ProductMappingEntity> InsertMany(List<ProductMappingEntity> ProductMappingEntityObjectList)
        {
            foreach (var ProductMappingEntityObject in ProductMappingEntityObjectList)
            {
                ProductMappingEntityObject.Id = Guid.NewGuid();
            }
            base.DB.Execute("usp_ProductMapping_InsertMany", ProductMappingEntityObjectList);

            return ProductMappingEntityObjectList;
        }

        public void LinkProductMappingToPackage(ProductMappingEntity ProductMappingEntityObject, PackageEntity packageEntityObject)
        {
            var ProductMappingPackage = new { Id = Guid.NewGuid(), ProductMappingId = ProductMappingEntityObject.Id, PackageId = packageEntityObject.Id };
            base.DB.Execute("usp_ProductMappingPackage_Insert", ProductMappingPackage);

        }

        public void LinkManyProductMappingToPackage(List<ProductMappingEntity> ProductMappingEntityObjectList, PackageEntity packageEntity)
        {
            List<object> ProductMappingPackageList = new List<object>();
            foreach (var ProductMappingEntityObject in ProductMappingEntityObjectList)
            {
                ProductMappingPackageList.Add(new { Id = Guid.NewGuid(), ProductMappingId = ProductMappingEntityObject.Id, PackageId = packageEntity.Id });
            }

            base.DB.Execute("usp_ProductMappingPackage_InsertMany", ProductMappingPackageList);

        }
    }
}
