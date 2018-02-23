using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.WebApi.Models.RecruiterBox
{
    internal sealed class RecruiterBoxOpening : ApiJobOpening
    {
        [JsonProperty(PropertyName = "id")]
        public override int Id { set; get; }

        [JsonProperty(PropertyName = "title")]
        public override string Title { set; get; }

        [JsonProperty(PropertyName = "description")]
        public override string Description { set; get; }

        [JsonProperty(PropertyName = "team")]
        public override string Team { set; get; }

        [JsonProperty(PropertyName = "state")]
        public override string State { set; get; }

        [JsonProperty(PropertyName = "position_type")]
        public override string PositionType { set; get; }

        [JsonProperty(PropertyName = "is_remote_allowed")]
        public override bool RemoteAvailable { set; get; }

        [JsonProperty(PropertyName = "is_private")]
        public override bool IsPrivate { set; get; }

        [JsonProperty(PropertyName = "hosted_url")]
        public override string HostedURL { set; get; }

        [JsonProperty(PropertyName = "application_email")]
        public override string ApplicationEmail { set; get; }

        [JsonProperty(PropertyName = "is_archived")]
        public override bool IsArhived { set; get; }

        [JsonProperty(PropertyName = "created_date")]
        public override DateTime DateCreated { set; get; }

        [JsonProperty(PropertyName = "modified_date")]
        public override DateTime DateModified { set; get; }
    }
}
