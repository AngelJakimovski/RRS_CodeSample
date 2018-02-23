using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AARP.Models.Evaluations
{
    public class StatsInterviewersViewModel
    {
        public int Id { get; set; }

        [DisplayName("Email")]
        public string InterviewerEmail { get; set; }

        [DisplayName("Interview date")]
        public DateTime InterviewDate { get; set; }

        [DisplayName("Name")]
        public string InterviewerName { get; set; }
    }
}