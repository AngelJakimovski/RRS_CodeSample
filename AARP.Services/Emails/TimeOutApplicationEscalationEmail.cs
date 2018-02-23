using AARP.Models;
using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Services.Emails
{
    public class TimeOutApplicationEscalationEmail : Postal.Email
    {
        public JobApplication JobApplication { get; set; }
        public Reviewer Reviewer { get; set; }
        public string EscalationTo { get; set; }
    }
}