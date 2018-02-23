using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.WebApi.Models.RecruiterBox
{
    internal sealed class RecruiterBoxInterview:ApiInterview
    {
        [JsonProperty(PropertyName = "id")]
        public override int Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public override string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public override string Description { get; set; }

        [JsonProperty(PropertyName = "time")]
        public override DateTime Time { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public override int Duration { get; set; }

        [JsonProperty(PropertyName = "location")]
        public override string Location { get; set; }

        [JsonProperty(PropertyName = "type")]
        public override object Type { get; set; }

        [JsonProperty(PropertyName = "candidate")]
        public override Dictionary <string, int> Candidate { get; set; }

        [JsonProperty(PropertyName = "created_by")]
        public RecruiterBoxUser2 CreatedBy { get; set; }

        [JsonProperty(PropertyName = "date_created")]
        public override DateTime DateCreated { get; set; }

        [JsonProperty(PropertyName = "is_private")]
        public override bool IsPrivate { get; set; }

        [JsonProperty(PropertyName = "is_cancelled")]
        public override bool IsCancelled { get; set; }

        [JsonProperty(PropertyName = "invitees")]
        public override List<Dictionary<string,string>> Invitees { get; set; }

        [JsonProperty(PropertyName = "messages")]
        public List<string> Messages { get; set; }

    }
}
