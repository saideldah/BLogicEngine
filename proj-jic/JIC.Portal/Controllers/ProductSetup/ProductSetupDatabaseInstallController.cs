using BSynchro.Web.Controllers;
using JIC.Business.Model;
using JIC.Business.ProductSetup.Manager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace JIC.Portal.Controllers.ProductSetup
{
    public class ProductSetupDatabaseInstallController : SetupControllerBase
    {
        #region Properties
        private readonly Lazy<ProductSetupManager> manager;
        #endregion

        #region Constructor
        public ProductSetupDatabaseInstallController()
        {
            manager = new Lazy<ProductSetupManager>(GetProductSetupManager);
        }
        #endregion

        #region Private Methods
        private ProductSetupManager GetProductSetupManager()
        {
            return new ProductSetupManager();
        }
        #endregion

        #region Public Methods
        protected override IEnumerable<string> GetResourceScripts()
        {
            return new List<string>();
        }


        protected override void InstallAppDatabaseSchema()
        {
            manager.Value.InstallSchema();
        }

        [HttpPost]
        public HttpResponseMessage Post()
        {
            try
            {
                InstallAppDatabaseSchema();
                var response = Request.CreateResponse(HttpStatusCode.OK, new ResponseModel() { Status = HttpStatusCode.OK.ToString(), Message = "Done" });
                return response;
            }
            catch (Exception e)
            {

                List<ErrorModel> errorModelList = new List<ErrorModel>();
                errorModelList.Add(new ErrorModel() { Type = e.GetType().ToString(), Message = e.Message });
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, new ResponseModel() { Status = HttpStatusCode.InternalServerError.ToString(), Message = e.Message, Data = errorModelList });
                return response;
            }
        }
        #endregion
    }
}