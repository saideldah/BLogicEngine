using Insight.BSynchroExtensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.DataAccess.ProductSetup
{
    public class ProductSetupDataBase
    {
        #region Private Fields
        private readonly string connectionString;
        #endregion

        #region Constructor
        public ProductSetupDataBase()
        {
            var connectionStringObject = ConfigurationManager.ConnectionStrings["ProductSetupConnection"];
            this.connectionString = connectionStringObject != null ? connectionStringObject.ConnectionString : null;
        }
        #endregion
        public void Install()
        {
            string scriptFilePaths = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "bin", "Controllers", "ProductSetup", "SqlScript", "ProductSetupDatabaseModel.sql");
            bool fileFound = false;
            if (File.Exists(scriptFilePaths))
            {
                fileFound = true;
                var sqlScript = File.ReadAllText(scriptFilePaths);
                var schemaManager = new DatabaseSchemaManager(connectionString);
                schemaManager.Setup(sqlScript, false, "JIC_ProductSetup");
            }
            if (!fileFound)
            {
                throw new FileNotFoundException(string.Format("ProductSetupDatabaseModel.sql was not found at the following paths: {0}", scriptFilePaths));
            }
        
        }

        public void Install(string script)
        {
            var schemaManager = new DatabaseSchemaManager(connectionString);
            schemaManager.Setup(script, false, "JIC_ProductSetup");
        }
    }
}
