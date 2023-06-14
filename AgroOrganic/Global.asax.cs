using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using AgroOrganic.Models;
using System.Data.Entity.SqlServer;
using Microsoft.SqlServer.Types;

namespace AgroOrganic
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            SqlProviderServices.SqlServerTypesAssemblyName = typeof(SqlGeography).Assembly.FullName;
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(App_Start.WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
