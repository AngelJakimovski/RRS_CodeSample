using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Models
{
    public class ApplicantViewModel
    {
        public JobApplication Applicant { get; set; }

        public Reviewer Reviewer { get; set; }
    }
}