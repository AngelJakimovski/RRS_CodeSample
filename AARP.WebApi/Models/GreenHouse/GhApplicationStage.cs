using RestSharp.Deserializers;

namespace AARP.WebApi.Models.GreenHouse
{
    internal sealed class GhApplicationStage: ApiApplicationStage
    {
        [DeserializeAs(Name = "id")]
        public override string Id { get; set; }

        [DeserializeAs(Name = "name")]
        public override string Name { get; set; }
    }
}