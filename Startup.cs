using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.SqlServer;
using AARP.Infrashtructure.DI;
using AARP.Services;
using System.IO;
using System.Threading;
using AARP.Infrashtructure.Commons;
using AARP.Infrashtructure.Logging;
using Microsoft.Owin.BuilderProperties;

[assembly: OwinStartup(typeof(AARP.Startup))]
namespace AARP
{
    public partial class Startup
    {
        log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(Startup));

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            AARP.App_Start.HangfireConfig.ConfigureHangefire(app);

            ConfigureDependencyInjection();

            InitGreenHouseData();
            InitBackgroundJobs();

            var properties = new AppProperties(app.Properties);
            CancellationToken token = properties.OnAppDisposing;
            if (token != CancellationToken.None)
            {
                token.Register(() =>
                {
                    GreenHouseDataStore.Instance.API.CleanUp();
                });
            }

            _logger.InfoFormat("Applicant has been started at {0}", DateTime.Now);
        }

        private void InitGreenHouseData()
        {
            GreenHouseDataStore.Instance.Initialize();
        }

        private static void ConfigureDependencyInjection()
        {
            DiContainers.Global.Install(new DiGlobalInstaller());
        }

        public void InitBackgroundJobs()
        {
            BackgroundJobsManager.Initialize();
        }
    }
}