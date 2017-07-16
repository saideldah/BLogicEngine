using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BSynchro.Web.Owin;

[assembly: OwinStartup(typeof(LovManager.Api.Startup))]

namespace LovManager.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAmsApp(WebApiConfig.Get());
        }
    }
}