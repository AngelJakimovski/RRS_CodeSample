using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Dashboard;
using AARP.Services;
using Microsoft.Owin;

namespace AARP.App_Start
{
    public class HangfireConfig
    {
        public static void ConfigureHangefire(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("DpScopicAARP");
            var options = new DashboardOptions
            {
                AuthorizationFilters = new List<IAuthorizationFilter>()
                {
                    new AuthorizationFilter { Roles = "Admin" },
                    //new ClaimsBasedAuthorizationFilter("hangfire", "value")
                },
                AppPath = VirtualPathUtility.ToAbsolute("~")
            };

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", options);
        }

        //public static void StartOrUpdateJobs()
        //{
        //    var configs = AARP.Models.CommonAppConfigurations.Instance;
        //    var distributingJobId = AARP.Properties.Settings.Default.RecuringJob_SendEmailOfNewApplication;
        //    var updatingJobId = AARP.Properties.Settings.Default.RecuringJob_UpdateApplicationStatus;
        //    var remindJobId = AARP.Properties.Settings.Default.RecuringJob_SendEmailOfRemindReview;
        //    var escalatingJobId = AARP.Properties.Settings.Default.RecuringJob_SendEscalationEmailOfTimeOutApplication;

        //    //Distributing
        //    if (configs.DistributingApplications_IsEnabled)
        //        RecurringJob.AddOrUpdate(distributingJobId,() => BackgroundServices.DistributeApplicantsToReviewers(), configs.DistributingApplications_Interval);
        //    else
        //        RecurringJob.RemoveIfExists(distributingJobId);

        //    //Updating
        //    if (configs.UpdateApplicationStatus_IsEnabled)
        //        RecurringJob.AddOrUpdate(updatingJobId, () => BackgroundServices.RefreshStateOfApplicants(), configs.UpdateApplicationStatus_Interval);
        //    else
        //        RecurringJob.RemoveIfExists(updatingJobId);
        //}
    }

}