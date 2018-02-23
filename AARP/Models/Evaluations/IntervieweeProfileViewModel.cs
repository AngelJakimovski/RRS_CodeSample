using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Models.Evaluations
{
    public class IntervieweeProfileViewModel
    {
        public int IntervieweeID { set; get; }
        public string IntervieweeName { set; get; }
        public string Seniority { set; get; }
        public string JobApplicationName { set; get; }
        public IntervieweeResponseViewModel Interview1 { set; get; }
        public IntervieweeResponseViewModel Interview2 { set; get; }
    }

    public class IntervieweeResponseViewModel
    {
        public string InterviewerName { set; get;}
        public SurveyViewModel Survey { set; get; }
    }
}