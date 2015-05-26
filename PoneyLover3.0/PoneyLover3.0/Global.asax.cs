using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PoneyLover3._0
{
   public class MvcApplication : System.Web.HttpApplication
   {
      protected void Application_Start()
      {
         AreaRegistration.RegisterAllAreas();
         FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
         RouteConfig.RegisterRoutes(RouteTable.Routes);
         BundleConfig.RegisterBundles(BundleTable.Bundles);
      }

      protected void Session_Start()
      {
          Session["UserValid"] = false; // test
          Session["Username"] = "";
          string DB_Path = Server.MapPath(@"~\App_Data\DBPony.mdf");
          Session["DBPony"] = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='" + DB_Path + "'; Integrated Security=true";
      }
   }
}
