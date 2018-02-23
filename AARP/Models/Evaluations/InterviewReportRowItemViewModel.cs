using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Models.Evaluations
{
    public class InterviewReportRowItemViewModel
    {
        public int InterviewId { get; set; }

        public string InterviewerName { get; set; }

        public string Email { get; set; }

        public DateTime Date { get; set; }

        public string IntervieweeName { get; set; }

        public string FeedbackStatus { get; set; }

        public string Stage { get; set; }

        public string Seniority { get; set; }

        public string Link { get; set; }
    }
}