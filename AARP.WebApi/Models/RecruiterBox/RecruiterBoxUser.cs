using System.Collections.Generic;
using Newtonsoft.Json;

namespace AARP.WebApi.Models.RecruiterBox
{
    public sealed class RecruiterBoxUser: ApiUser
    {
        [JsonProperty(PropertyName = "id")]
        public override string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public override string Name { get; set; }

        [JsonProperty(PropertyName = "is_active")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        public override bool Disabled
        {
            get { return !IsActive; }
            set { IsActive = !value; }
        }

        public override List<string> Emails
        {
            get { return new List<string>() {Email}; }
            set
            {
                if (value != null)
                {
                    Email = value[0];
                }
            }
        }
    }
}
