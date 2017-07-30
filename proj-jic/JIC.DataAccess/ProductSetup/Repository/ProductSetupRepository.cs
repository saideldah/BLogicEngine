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
    abstract public class ProductSetupRepository : NonEntityRepository<object, Guid>
    {
        protected override string TableName { get { return ""; } }
        public static ProductSetupRepository CreateInstance()
        {
            ProductSetupRepository ProductSetupRepository = RepositoryFactory.Create<ProductSetupRepository>("ProductSetupConnection");
            return ProductSetupRepository;
        }


        public void DeleteAll()
        {
            base.DB.Execute("usp_ProductSetup_DeleteAll");
        }

       
    }
}
