namespace AARP.WebApi.Models.RecruiterBox
{
    using System;
    using Newtonsoft.Json;

    class RecruiterBoxApplicationStateMetadata
    {
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime? LastUpdateDate { get; set; }

        [JsonProperty(PropertyName = "state_comments")]
        public string Comment { get; set; }
    }
}
