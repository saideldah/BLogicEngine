using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JIC.Portal
{
    internal static class SiteConstants
    {
        public static String ConnectionString
        {
            get
            {
                //TODO: change the connection string name in case you are using a different one
                String connectionStringName = "DefaultConnection";
                return connectionStringName;
            }
        }
    }
}