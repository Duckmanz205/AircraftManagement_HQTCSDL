using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace QuanLyMayBay
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // --- SỬA: ĐƯA DÒNG NÀY LÊN TRÊN CÙNG (TRƯỚC RouteConfig) ---
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // -----------------------------------------------------------

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // RouteConfig phải nằm sau WebApiConfig
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}