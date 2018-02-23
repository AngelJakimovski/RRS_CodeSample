using AARP.Models;
using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AARP.WebApi.Models;

namespace AARP.Services.Emails
{
    public class RemindReviewApplicationEmail : Postal.Email
    {
        public JobApplication JobApplication { get; set; }
        public Reviewer Reviewer { get; set; }
        public ApiJob Job { get; set; }
    }
}