using System;
using System.Collections.Generic;
using System.Linq;

namespace AARP.WebApi.Models
{
    public abstract class ApiJob
    {
        public abstract string Id { get; set; }
        public abstract string Name { get; set; }
        public abstract string Status { get; set; }
        public abstract DateTime? CreatedAt { get; set; }
        public abstract Dictionary<string, string> CustomFields { get; set; }

        public string GetCustomFieldValue(string key)
        {
            if (CustomFields == null || CustomFields.Count == 0 || !CustomFields.Keys.Contains(key))
                return UnknownCustomValue;

            return CustomFields[key];
        }

        public const string UnknownCustomValue = "Unknown";
    }
}
