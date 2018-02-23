using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services.AppConfigs
{
    public class CommonConfigs : ConfigSection
    {
        [Required]
        public DayOfWeek[] EnabledDays { get; set; }

        [Required]
        public TimeSpan StartTimeOfDay { get; set; }

        [Required]
        public TimeSpan EndTimeOfDay { get; set; }
    }
}
