namespace AARP.WebApi.Models.RecruiterBox
{
    using System;
    using Newtonsoft.Json;

    internal class RecruiterBoxStageMovement
    {
        [JsonProperty(PropertyName = "is_stage_deleted")]
        public bool IsDeleted { get; set; }
        [JsonProperty(PropertyName = "candidate")]
        public string CandidateLink { get;set; }
        [JsonProperty(PropertyName = "opening")]
        public string JobLink { get; set; }
        [JsonProperty(PropertyName = "moved_on")]
        public DateTime CreationDate { get; set; }
        [JsonProperty(PropertyName = "stage")]
        public string StageLink { get; set; }
        [JsonProperty(PropertyName = "position")]
        public int Position { get; set; }
    }
}