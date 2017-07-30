using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JIC.Portal
{
    public static class WebApiConfig
    {
        public static HttpConfiguration Get()
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                 name: "DefaultApi",
                 routeTemplate: "api/{controller}/{action}/{id}",
                 defaults: new { id = RouteParameter.Optional }
             );
            return config;
        }
    }
}
