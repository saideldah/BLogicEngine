using BSynchro.Web;
using BSynchro.Web.Administration;
using BSynchro.Web.Localization;
using BSynchro.Web.Optimization;
using BSynchro.Web.Routing;
using BSynchro.Web.Security;
using System;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace JIC.Portal
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                AreaRegistration.RegisterAllAreas();

                // Initialize Web Optimization bundles
                AmsModulesManager.Instance.BuildModuleBundles();

                //responsible for caching metadata about controllers
                WebInitializationManager.Initialize(true);
                WebInitializationManager.InstallThemes();
                var resourceStore = new DefaultResourceStore(SiteConstants.ConnectionString);
                resourceStore.Initialize();
                IoC.Instance.Register<IResourceStore>(resourceStore);

                UniversalPrincipalStore.Instance.RefreshStore();

                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                   ((sender, certificate, chain, sslPolicyErrors) =>
                   {
                       return true;
                   });
            }
            catch (Exception ex)
            {
                ErrorHandler.CatchStartUpError(ex);
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            ErrorHandler.HandleStartUpError();
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            ErrorHandler.HandleError();
        }
    }
}