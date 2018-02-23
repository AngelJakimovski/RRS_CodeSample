using RestSharp.Deserializers;

namespace AARP.WebApi.Models.GreenHouse
{
    internal sealed class GhCandidateAttachment
    {
        [DeserializeAs(Name = "filename")]
        public string FileName { get; set; }

        [DeserializeAs(Name = "url")]
        public string Url { get; set; }

        [DeserializeAs(Name = "type")]
        public string Type { get; set; }
    }
}
