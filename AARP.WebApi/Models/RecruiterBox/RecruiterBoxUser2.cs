using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.WebApi.Models.RecruiterBox
{
    public sealed class RecruiterBoxUser2 : ApiUser2
    {
        [JsonProperty(PropertyName = "id")]
        public override string Id { set; get; }

        [JsonProperty(PropertyName = "name")]
        public override string Name { set; get; }

        [JsonProperty(PropertyName = "email")]
        public override string Email { set; get; }
    }
}
