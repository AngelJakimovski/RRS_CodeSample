using AARP.Services.Commands;
using Hangfire;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services
{
    public static class BackgroundJobsManager
    {
        private const string DistributeApplicantsJobId = "1";
        private const string RefreshApplicantsStateJobId = "2";
        private const string SyncEvaluationsJobId = "3";

        public static void Initialize()
        {
            if (AppConfigsProvider.DistributeApplicantsConfigs.IsEnabled)
            {
                var interval = AppConfigsProvider.DistributeApplicantsConfigs.Recurrence;
                RecurringJob.AddOrUpdate(DistributeApplicantsJobId, () => DistributeApplicantsToReviewers(), interval);
            }
            else
                RecurringJob.RemoveIfExists(DistributeApplicantsJobId);

            if (AppConfigsProvider.RefreshApplicantsConfigs.IsEnabled)
            {
                var interval = AppConfigsProvider.RefreshApplicantsConfigs.Recurrence;
                RecurringJob.AddOrUpdate(RefreshApplicantsStateJobId, () => RefreshAssignedApplicantsState(), interval);
            }
            else
                RecurringJob.RemoveIfExists(RefreshApplicantsStateJobId);

            if (AppConfigsProvider.SyncEvaluations.IsEnabled)
            {
                RecurringJob.AddOrUpdate(SyncEvaluationsJobId, () => RefreshEvaluationsData(), Cron.Hourly);
            }
            else
            {
                RecurringJob.RemoveIfExists(SyncEvaluationsJobId);
            }

            if (AppConfigsProvider.SendEvaluationReport.IsEnabled)
            {
                RecurringJob.AddOrUpdate(AppConfigsProvider.SendEvaluationReport.Name, () => SendEvaluationsReportEmail(), Cron.Weekly());
            }
            else
            {
                RecurringJob.RemoveIfExists(AppConfigsProvider.SendEvaluationReport.Name);

            }
        }

        [LogFailure]
        [DisplayName("Search and distribute new applicants to reviewers")]
        public static void DistributeApplicantsToReviewers()
        {
            Queue<ICommand> commands = new Queue<ICommand>(new ICommand[]{
                new Commands.PullingNewApplicantsJob(),
                new Commands.DistributeNewApplicants()
            });

            while (commands.Count() != 0)
            {
                var nextCommand = commands.Dequeue();
                if (nextCommand.CanExecute())
                    nextCommand.Execute();
            }
        }

        [LogFailure]
        [DisplayName("Refresh/Update status of assigned applicants")]
        public static void RefreshAssignedApplicantsState()
        {
            Queue<ICommand> commands = new Queue<ICommand>(new ICommand[]{
                new Commands.UpdateApplicantsStatus(),
                new Commands.RemindReviewers(),
                new Commands.EscalateUnhandledApplicants()
            });

            while (commands.Count() != 0)
            {
                var nextCommand = commands.Dequeue();
                if (nextCommand.CanExecute())
                    nextCommand.Execute();
            }
        }

        [LogFailure]
        [DisplayName("Refresh/Update Evaluations")]
        public static void RefreshEvaluationsData()
        {
            Queue<ICommand> commands = new Queue<ICommand>(new ICommand[]{
                new Commands.ReviewsSyncronizer()
            });

            while (commands.Count() != 0)
            {
                var nextCommand = commands.Dequeue();
                if (nextCommand.CanExecute())
                    nextCommand.Execute();
            }
        }

        [LogFailure]
        [DisplayName("Send periodical interviews report information")]
        public static void SendEvaluationsReportEmail()
        {
            Queue<ICommand> commands = new Queue<ICommand>(new ICommand[]{
                new Commands.ReportInterviewEvaluations()
            });

            while (commands.Count() != 0)
            {
                var nextCommand = commands.Dequeue();
                if (nextCommand.CanExecute())
                    nextCommand.Execute();
            }
        }

    }
}
