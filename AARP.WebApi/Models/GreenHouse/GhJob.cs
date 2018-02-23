using System;
using System.Collections.Generic;
using AARP.WebApi.Commons;
using RestSharp.Deserializers;

namespace AARP.WebApi.Models.GreenHouse
{
    internal sealed class GhJob: ApiJob
    {
        private Dictionary<string, string> _customFields;

        public GhJob()
        {
            _customFields = new Dictionary<string, string>();
        }

        [DeserializeAs(Name = "id")]
        public override string Id { get; set; }

        [DeserializeAs(Name = "name")]
        public override string Name { get; set; }

        [DeserializeAs(Name = "requisition_id")]
        public string RequisitionId { get; set; }

        [DeserializeAs(Name = "notes")]
        public string Notes { get; set; }

        [DeserializeAs(Name = "status")]
        public override string Status { get; set; }

        [DeserializeAs(Name = "created_at")]
        public override DateTime? CreatedAt { get; set; }

        [DeserializeAs(Name = "opened_at")]
        public DateTime? OpenedAt { get; set; }

        [DeserializeAs(Name = "closed_at")]
        public DateTime? ClosedAt { get; set; }

        [DeserializeAs(Name = "departments")]
        public List<string> Departments { get; set; }

        [DeserializeAs(Name = "offices")]
        public List<string> Offices { get; set; }

        [DeserializeAs(Name = "custom_fields")]
        public override Dictionary<string, string> CustomFields
        {
            get
            {
                if (_customFields != null)
                {
                    if (!_customFields.ContainsKey(JobCustomFields.ResourceUrl))
                    {
                        _customFields.Add(JobCustomFields.ResourceUrl, Id);
                    }
                    else
                    {
                        _customFields[JobCustomFields.ResourceUrl] = Id;
                    }
                }
                return _customFields;
            }
            set
            {
                if (value != null)
                {
                    _customFields = value;
                }
            }
        }
    }
}