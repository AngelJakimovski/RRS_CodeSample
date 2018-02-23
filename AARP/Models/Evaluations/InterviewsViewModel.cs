using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Models.Evaluations
{
    public class InterviewViewModel
    {
        public string Interviewer { set; get;}
        public DateTime InterviewDate { set; get; }
        public string Interviewee { set; get; }
        public int IntervieweeID { set; get; }
        public string InterviewStage { set; get; }
        public string Seniority { set; get; }
        public string InterviewStatus { set; get; }
        public decimal Rating { set; get; }
    }
    public class InterviewsViewModel
    {
        public List<InterviewViewModel> Interviews { set; get; }
        
        public InterviewsViewModel()
        {            
            Interviews = new List<InterviewViewModel>();
        }
    
    }

    public class InterviewsFiltersViewModel
    {
        public Dictionary<InterviewStage, bool> StageFilter { set; get; }
        public Dictionary<Seniority, bool> SeniorityFilter { set; get; }

        public InterviewsFiltersViewModel()
        {
            StageFilter= new Dictionary<InterviewStage, bool>();
            SeniorityFilter = new Dictionary<Seniority, bool>();
        }
    }

    public class InterviewsFiltersPostViewModel
    {
        public InterviewsFiltersPostViewModel()
        {
            // setting default values
            dateFrom = DateTime.Now.AddDays(-30);
            dateTo = DateTime.Now;
        }

        public List<int> selectedstages { set; get; }
        public List<int> selectedseniorities { set; get; }
        public DateTime? dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
    }
}