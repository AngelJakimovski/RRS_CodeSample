using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services.AppConfigs
{
    public class ReportErrorsConfig: ConfigSection
    {
        public bool IsEnabled { get; set; }

        public string ReportToEmails { get; set; }
    }
}
