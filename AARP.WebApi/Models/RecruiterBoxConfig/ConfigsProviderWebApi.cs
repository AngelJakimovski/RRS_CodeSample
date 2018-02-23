using AARP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AARP.WebApi.Models.RecruiterBoxConfig
{
    public static class ConfigsProviderWebApi
    {
        public static TConfigs GetConfigs<TConfigs>()
            where TConfigs : ConfigSectionWebApi
        {
            AARP.Models.Configuration cnfgEntity = null;
            var key = "RecruiterBoxConfigs";

            using (var dbContext = new AARPDbContext())
            {
                cnfgEntity = dbContext.Configurations.FirstOrDefault(cfg => cfg.Key == key);
            }

            if (cnfgEntity != null)
            {
                return JsonConvert.DeserializeObject<TConfigs>(cnfgEntity.Value);
            }
            else
                return Activator.CreateInstance<TConfigs>();
        }
    }
}
