using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;
using log4net.Config;

namespace AARP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ConfigLogging();
        }


        private void ConfigLogging()
        {
            XmlConfigurator.Configure();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var errorLink = $"Error on page: {Request.RawUrl}";
            _logger.Error(errorLink, exception);
        }
    }
}
