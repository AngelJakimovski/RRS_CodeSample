using AARP.Models;
using Hangfire.States;
using Postal;

namespace AARP.Services.Emails
{
    public class EvaluationInvitationEmail:Email
    {
        public Interviewee interviewee { set; get; }
        public Interviewer interviewer { set; get; }
        public string EvaluationLink { set; get; }
    }
}
