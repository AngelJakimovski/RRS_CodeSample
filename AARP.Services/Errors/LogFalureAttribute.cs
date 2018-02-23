using AARP.Services.Emails;
using Hangfire.Common;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace AARP.Services
{
    public class LogFailureAttribute : JobFilterAttribute, IApplyStateFilter
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LogFailureAttribute));

        public void OnStateApplied(ApplyStateContext context, Hangfire.Storage.IWriteOnlyTransaction transaction)
        {
            var failedState = context.NewState as FailedState;
            if (failedState != null)
            {
                Logger.Error(
                    $"Background job #{context.BackgroundJob.Id} was failed with an exception.",
                    failedState.Exception);

                var configs = AppConfigsProvider.GetConfigs<AppConfigs.ReportErrorsConfig>();
                if (configs.IsEnabled)
                {
                    var email = new NotificationOfErrorEmail()
                    {
                        HangfireContext = context,
                        To = string.Join("; ", configs.ReportToEmails)
                    };
                    BackgroundEmailService.Create().Send(email);
                }
            }
        }


        public void OnStateUnapplied(ApplyStateContext context, Hangfire.Storage.IWriteOnlyTransaction transaction)
        {
        }
    }
}
