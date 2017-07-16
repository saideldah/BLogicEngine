using BSynchro.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BSynchro.Web.Optimization;
using BSynchro.Web.Security;
using BSynchro.Web.Localization;

namespace LovManager.Api
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Initialize Web Optimization bundles
            AmsModulesManager.Instance.BuildModuleBundles(true);

            WebInitializationManager.Initialize();
            //WebInitializationManager.InitializeEnterpriseServiceBusForApplication("appname", "/appname/api/appbus");

            UniversalPrincipalStore.Instance.RefreshStore();

            var resourceStore = new DefaultResourceStore(() => "DefaultConnection");
            resourceStore.Initialize();
            IoC.Instance.Register<IResourceStore>(resourceStore);
            IoC.Instance.Register<IResourceContainerProvider>(new DatabaseResourceContainerProvider());

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            System.Net.ServicePointManager.ServerCertificateValidationCallback =
            ((sender, certificate, chain, sslPolicyErrors) =>
            {
                return true;
            });


        }
    }
}