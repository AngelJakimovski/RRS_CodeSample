using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services.AppConfigs
{
    public class EscalateTimeOutApplicantsConfigs: ConfigSection
    {
        public bool IsEnabled { get; set; }

        public int RemindTheshold { get; set; }

        public string ReportToEmails { get; set; }

        public string[] GetReportToEmails()
        {
            return ReportToEmails.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(email => email.Trim().ToLower()).ToArray();
        }
    }
}
