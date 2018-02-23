using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AARP.WebApi.Models.RecruiterBox
{
    public sealed class RecruiterBoxCandidate: ApiCandidate
    {
        [JsonProperty(PropertyName = "id")]
        public override string Id { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public override string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public override string LastName { get; set; }
        
        [JsonProperty(PropertyName = "email")]
        public override string Email { get; set; }
        
        [JsonProperty(PropertyName = "labels")]
        public List<RecruiterBoxCandidateLabel> Labels { get; set; }

        [JsonProperty(PropertyName = "opening_id")]
        public override int OpeningID { get; set; }

        [JsonProperty(PropertyName = "stage_id")]
        public override int StageID { get; set; }

        [JsonProperty(PropertyName = "stage_name")]
        public override string StageName { get; set; }

        [JsonProperty(PropertyName = "state")]
        public override string State { get; set; }
    }
}
