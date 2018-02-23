using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services.AppConfigs
{
    public class RecruiterBoxConfigs : ConfigSection
    {

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
