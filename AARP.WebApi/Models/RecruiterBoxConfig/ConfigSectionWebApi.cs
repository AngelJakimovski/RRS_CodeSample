using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.WebApi.Models.RecruiterBoxConfig
{
    public abstract class ConfigSectionWebApi
    {
        public string Name { get { return this.GetType().Name; } }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
