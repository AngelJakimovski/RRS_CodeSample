
using AARP.Models;
using AARP.Services.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services
{
    public class EvaluationsEmailService
    {
        public void SendEvaluationInvitationEmail(int InterviewerID, int IntervieweeID)
        {
            using (var dbContext = new AARPDbContext())
            {
                var Interviewee = dbContext.Interviewees.Where(x => x.Id == IntervieweeID).First();
                var Interviewer = dbContext.Interviewers.Where(x => x.Id == InterviewerID).First();
                var email = new EvaluationInvitationEmail()
                {
                    interviewer = Interviewer,
                    interviewee = Interviewee,
                    EvaluationLink = GenerateEvaluationLink(IntervieweeID, InterviewerID)
                };

                BackgroundEmailService.Create().Send(email);
            }
        }

        private string GenerateEvaluationLink(int intervieweeID, int interviewerID)
        {
            return string.Format("/survey/{0}/{1}", intervieweeID, interviewerID);
        }
    }
}
