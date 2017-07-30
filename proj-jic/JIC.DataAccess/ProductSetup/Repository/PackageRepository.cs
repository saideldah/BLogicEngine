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
    abstract public class PackageRepository : NonEntityRepository<PackageEntity, Guid>
    {
        protected override string TableName { get { return "Package"; } }
        public static PackageRepository CreateInstance()
        {
            PackageRepository packageRepository = RepositoryFactory.Create<PackageRepository>("ProductSetupConnection");
            return packageRepository;
        }
        public PackageEntity Insert(PackageEntity PackageEntityObject)
        {
            PackageEntityObject.Id = Guid.NewGuid();
            base.DB.Execute("usp_Package_Insert", PackageEntityObject);
            return PackageEntityObject;
        }

        public List<PackageEntity> InsertMany(List<PackageEntity> packageEntityObjectList)
        {
            foreach (var packageEntityObject in packageEntityObjectList)
            {
                packageEntityObject.Id = Guid.NewGuid();
            }
            base.DB.Execute("usp_Package_InsertMany", packageEntityObjectList);

            return packageEntityObjectList;
        }

        public void LinkPackageToProperty(PackageEntity packageEntityObject, PropertyEntity propertyEntity)
        {
            var packageProperty = new { Id = Guid.NewGuid(), Value = propertyEntity.Value, PropertyId = propertyEntity.Id, PackageId = packageEntityObject.Id };
            base.DB.Execute("usp_PackageProperty_Insert", packageProperty);

        }

        public void LinkPackageToManyProperty(PackageEntity packageEntityObject, List<PropertyEntity> propertyEntityObjectList)
        {
            List<object> packagePropertyList = new List<object>();
            foreach (var propertyEntityObject in propertyEntityObjectList)
            {
                packagePropertyList.Add(new { Id = Guid.NewGuid(), Value = propertyEntityObject.Value, PropertyId = propertyEntityObject.Id, PackageId = packageEntityObject.Id });
            }
            base.DB.Execute("usp_PackageProperty_InsertMany", packagePropertyList);

        }

        public void LinkPackageToProduct(PackageEntity packageEntity, ProductEntity productEntityObject)
        {

            var productPackage = new { Id = Guid.NewGuid(), ProductId = productEntityObject.Id, PackageId = packageEntity.Id };
            base.DB.Execute("usp_ProductPackage_Insert", productPackage);

        }
        public void LinkPackageToManyProduct(PackageEntity packageEntity, List<ProductEntity> productEntityObjectList)
        {
            List<object> productPackageList = new List<object>();
            foreach (var productEntityObject in productEntityObjectList)
            {
                productPackageList.Add(new { Id = Guid.NewGuid(), ProductId = productEntityObject.Id, PackageId = packageEntity.Id });
            }

            base.DB.Execute("usp_ProductPackage_InsertMany", productPackageList);

        }
    }
}
