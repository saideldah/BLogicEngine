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
    abstract public class ProductSetupFileRepository : NonEntityRepository<object, Guid>
    {
        protected override string TableName { get { return "ProductSetupFile"; } }
        public static ProductSetupFileRepository CreateInstance()
        {
            ProductSetupFileRepository ProductSetupRepository = RepositoryFactory.Create<ProductSetupFileRepository>("ProductSetupConnection");
            return ProductSetupRepository;
        }


        public void DeleteAll()
        {
            base.DB.Execute("usp_ProductSetup_DeleteAll");
        }
        public ProductSetupFileEntity Insert(ProductSetupFileEntity productSetupFileEntity)
        {
            productSetupFileEntity.Id = Guid.NewGuid();
            productSetupFileEntity.CreatedDate = DateTime.Now;
            productSetupFileEntity.ModifiedDate = DateTime.Now;
            base.DB.Execute("usp_ProductSetupFile_Insert", productSetupFileEntity);

            return productSetupFileEntity;
        }

        public List<ProductSetupFileEntity> SelectAll(ProductSetupFileEntity productSetupFileEntity)
        {
            return base.DB.Query<ProductSetupFileEntity>("usp_ProductSetupFile_Select", productSetupFileEntity).ToList();
        }

        public List<ProductSetupFileEntity> SelectAll()
        {
            return base.DB.Query<ProductSetupFileEntity>("usp_ProductSetupFile_SelectAll").ToList();
        }


    }
}
