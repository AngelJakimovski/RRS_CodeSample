using AARP.Models;
using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services.Emails
{
    public class InterviewsEvaluationReportEmail : Email
    {
        public InterviewsEvaluationReportEmail()
        {
            InterviewReportItems = new List<EvaluationReportItem>();
        }

        public List<EvaluationReportItem> InterviewReportItems { get; set; }
    }

    public class EvaluationReportItem
    {
        public Interviewee interviewee { set; get; }
        public Interviewer interviewer { set; get; }
        public Interview interview { get; set; }
        public InterviewStage stage { get; set; }
        public Seniority seniority { get; set; }
        //public InterviewRating rating { set; get; }
    }
}
