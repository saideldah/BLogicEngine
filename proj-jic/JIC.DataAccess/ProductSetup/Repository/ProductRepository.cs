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
    abstract public class ProductRepository : NonEntityRepository<ProductEntity, Guid>
    {
        protected override string TableName { get { return "Product"; } }
        public static ProductRepository CreateInstance()
        {
            ProductRepository productRepository = RepositoryFactory.Create<ProductRepository>("ProductSetupConnection");
            return productRepository;
        }
        public ProductEntity Insert(ProductEntity productEntityObject)
        {
            productEntityObject.Id = Guid.NewGuid();
            base.DB.Execute("usp_Product_Insert", productEntityObject);
            return productEntityObject;
        }

        public List<ProductEntity> InsertMany(List<ProductEntity> productEntityObjectList)
        {
            foreach (var productEntityObject in productEntityObjectList)
            {
                productEntityObject.Id = Guid.NewGuid();
            }
            base.DB.Execute("usp_Product_InsertMany", productEntityObjectList);

            return productEntityObjectList;
        }

        public void LinkProductToPackage(ProductEntity productEntityObject, PackageEntity packageEntityObject)
        {
            var productPackage = new { Id = Guid.NewGuid(), ProductId = productEntityObject.Id, PackageId = packageEntityObject.Id };
            base.DB.Execute("usp_ProductPackage_Insert", productPackage);

        }

        public void LinkManyProductToPackage(List<ProductEntity> productEntityObjectList, PackageEntity packageEntity)
        {
            List<object> productPackageList = new List<object>();
            foreach (var productEntityObject in productEntityObjectList)
            {
                productPackageList.Add(new { Id = Guid.NewGuid(), ProductId = productEntityObject.Id, PackageId = packageEntity.Id });
            }
            
            base.DB.Execute("usp_ProductPackage_InsertMany", productPackageList);

        }


        public void LinkProductToCoverProperty(ProductEntity productEntityObject, CoverEntity coverEntityObject, PropertyEntity propertyEntity)
        {
            var productPackage = new { Id = Guid.NewGuid(), ProductId = productEntityObject.Id, CoverId = coverEntityObject.Id, PropertyId = propertyEntity.Id, Value = propertyEntity.Value };
            base.DB.Execute("usp_ProductCoverProperty_Insert", productPackage);

        }


    }
}
