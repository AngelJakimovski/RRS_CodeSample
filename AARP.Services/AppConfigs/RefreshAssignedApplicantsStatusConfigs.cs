using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services.AppConfigs
{
    public class RefreshAssignedApplicantsStatusConfigs : ConfigSection
    {
        public string Recurrence { get; set; }

        public bool IsEnabled { get; set; }
    }
}
