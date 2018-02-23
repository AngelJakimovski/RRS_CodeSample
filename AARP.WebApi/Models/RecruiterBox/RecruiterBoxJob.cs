using System;
using System.Collections.Generic;
using System.Linq;
using AARP.WebApi.Commons;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace AARP.WebApi.Models.RecruiterBox
{
    internal sealed class RecruiterBoxJob: ApiJob
    {
        private Dictionary<string, string> _customFields;

        public RecruiterBoxJob()
        {
            _customFields = new Dictionary<string, string>()
            {
                {JobCustomFields.ResourceUrl, string.Empty},
                {JobCustomFields.PositionType, string.Empty}
            };
        }

        [DeserializeAs(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public override string Id { get; set; }

        [DeserializeAs(Name = "title")]
        [JsonProperty(PropertyName = "name")]
        public override string Name { get; set; }
        
        [DeserializeAs(Name = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "resource_uri")]
        public string ResourceUri { get; set; }
        
        [DeserializeAs(Name = "created_on")]
        [JsonProperty(PropertyName = "created_on")]
        public override DateTime? CreatedAt { get; set; }

        public override Dictionary<string, string> CustomFields
        {
            get
            {
                _customFields[JobCustomFields.ResourceUrl] = ResourceUri;
                _customFields[JobCustomFields.PositionType] = JobPositionType;
                return _customFields;
            }
            set { _customFields = value; }
        }

        [DeserializeAs(Name = "is_archived")]
        [JsonProperty(PropertyName = "is_archived")]
        public bool IsArchived { get; set; }

        public override string Status
        {
            get
            {
                return !IsArchived ? JobStates.Open : JobStates.Closed;
            }
            set
            {
                if (value == JobStates.Open) IsArchived = false;
                if (value == JobStates.Closed) IsArchived = true;
            }
        }
        
        [DeserializeAs(Name = "team")]
        public string Team { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public List<RecruiterBoxJobTag> Tags { get; set; }

        public string JobPositionType
        {
            get
            {
                if (Tags == null) return null;
                const string positionTypeLabel = "PT: ";
                var positionTypeTag =
                    Tags.FirstOrDefault(tag => tag.Name.IndexOf(positionTypeLabel, StringComparison.Ordinal) == 0);
                return positionTypeTag?.Name.Remove(0, positionTypeLabel.Length);
            }
        }
    }
}