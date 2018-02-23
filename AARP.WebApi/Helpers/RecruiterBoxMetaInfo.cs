using Newtonsoft.Json;

namespace AARP.WebApi.Helpers
{
    class RecruiterBoxMetaInfo
    {
        [JsonProperty(PropertyName = "limit")]
        public int Limit { get; set; }
        [JsonProperty(PropertyName = "offset")]
        public int Offset { get; set; }
        [JsonProperty(PropertyName = "total_count")]
        public int TotalCount { get; set; }
        [JsonProperty(PropertyName = "next")]
        public string NextUrl { get; set; }
        [JsonProperty(PropertyName = "previous")]
        public string PreviousUrl { get; set; }
    }
}
