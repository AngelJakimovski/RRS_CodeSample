using AARP.Models;
using AARP.Services.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services.Commands
{
    public class ReportInterviewEvaluations : ICommand
    {
        public bool CanExecute()
        {
            return AppConfigsProvider.SendEvaluationReport.IsEnabled;
        }

        public void Execute()
        {
            var report = new InterviewsEvaluationReportEmail();

            using (var dbContext = new AARPDbContext())
            {
                var lastDate = DateTime.Now.AddDays(-7);

                report.InterviewReportItems = (from i in dbContext.Interviews
                                               join ivr in dbContext.Interviewers on i.InterviewerId equals ivr.Id
                                               join iw in dbContext.Interviewees on i.IntervieweeId equals iw.Id
                                               join st in dbContext.InterviewStages on i.InterviewStageId equals st.Id
                                               join sn in dbContext.Seniorities on i.SeniorityId equals sn.Id
                                               where i.Date >= lastDate
                                               orderby i.Date descending
                                               select new EvaluationReportItem
                                               {
                                                   interview = i,
                                                   interviewee = iw,
                                                   interviewer = ivr,
                                                   seniority = sn,
                                                   stage = st
                                               }).ToList();
            }

            BackgroundEmailService.Create().Send(report);
        }
    }
}
