using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.WebApi.Models.RecruiterBoxConfig
{
    public class ConfigurationsModelWebApi
    {
        public RecruiterBoxConfigsWebApi RecruiterBoxConfigs { get; set; }

        public ConfigurationsModelWebApi()
        {
            this.RecruiterBoxConfigs = ConfigsProviderWebApi.GetConfigs<RecruiterBoxConfigsWebApi>();
        }
    }
}
