using Newtonsoft.Json;
namespace AARP.WebApi.Models.RecruiterBox
{
    internal sealed class RecruiterBoxApplicationStage: ApiApplicationStage
    {
        [JsonProperty(PropertyName =  "id")]
        public override string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public override string Name { get; set; }
    }
}
