using RestSharp.Deserializers;

namespace AARP.WebApi.Models.GreenHouse
{
    internal sealed class GhApplicationSource: ApiApplicationSource
    {
        [DeserializeAs(Name = "id")]
        public override int Id { get; set; }

        [DeserializeAs(Name = "public_name")]
        public override string Name { get; set; }
    }
}
