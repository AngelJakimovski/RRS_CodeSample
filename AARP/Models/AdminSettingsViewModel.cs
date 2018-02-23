using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AARP.Models
{
    public class AdminSettingsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Set timer for sending email")]
        public int EmailTimer { get; set; }

        [Display(Name = "Edit Feedback Message ")]
        public string FeedbackMessage { get; set; }

        [Display(Name = "Set reminder")]
        public int EnableReminder { get; set; }

        [Display(Name = "Edit Reminder Message ")]
        public string ReminderMessage { get; set; }
    }

    public class AdminSettingsPageViewModel
    {
        AdminSettingsViewModel TechInterview1 { set; get; }
        AdminSettingsViewModel TechInterview2 { set; get; }
    }
}