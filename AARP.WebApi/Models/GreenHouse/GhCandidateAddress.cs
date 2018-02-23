﻿using RestSharp.Deserializers;

namespace AARP.WebApi.Models.GreenHouse
{
    internal sealed class GhCandidateAddress
    {
        [DeserializeAs(Name = "type")]
        public string Type { get; set; }

        [DeserializeAs(Name = "value")]
        public string Value { get; set; }

    }
}