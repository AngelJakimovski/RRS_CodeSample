using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.GreenHouseAPI.Models
{
    public class RecruiterBoxJob
    {
        [DeserializeAs(Name = "id")]
        public int Id { get; set; }

        [DeserializeAs(Name = "title")]
        public string Title { get; set; }

        /*[DeserializeAs(Name = "requisition_id")]
        public string RequisitionId { get; set; }

        [DeserializeAs(Name = "notes")]
        public string Notes { get; set; }

        [DeserializeAs(Name = "status")]
        public string Status { get; set; }*/

        [DeserializeAs(Name = "created_on")]
        public DateTime? CreatedOn { get; set; }

        /*[DeserializeAs(Name = "opened_at")]
        public DateTime? OpenedAt { get; set; }

        [DeserializeAs(Name = "closed_at")]
        public DateTime? ClosedAt { get; set; }*/

        [DeserializeAs(Name = "team")]
        public string Team { get; set; }

        /*[DeserializeAs(Name = "offices")]
        public List<string> Offices { get; set; }

        [DeserializeAs(Name = "custom_fields")]
        public Dictionary<string, string> CustomFields { get; set; }

        public string GetCustomFieldValue(string key)
        {
            string unknown = "Unknown";
            if (CustomFields == null || CustomFields.Count == 0 || !CustomFields.Keys.Contains(key))
                return unknown;

            return CustomFields[key];
        }*/
    }
}