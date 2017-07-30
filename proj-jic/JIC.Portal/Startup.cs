using Microsoft.Owin;
using Owin;
using BSynchro.Web.Owin;

[assembly: OwinStartup(typeof(JIC.Portal.Startup))]

namespace JIC.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAmsPortal(WebApiConfig.Get());
        }
    }
}