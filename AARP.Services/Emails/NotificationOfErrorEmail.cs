using Hangfire.States;
using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Services.Emails
{
    public class NotificationOfErrorEmail : Email
    {
        public string To { get; set; }

        public ApplyStateContext HangfireContext { get; set; }
    }
}