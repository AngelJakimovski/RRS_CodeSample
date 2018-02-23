using System.Collections.Generic;
using RestSharp.Deserializers;

namespace AARP.WebApi.Models.GreenHouse
{
    internal sealed class GhGreenHouseUser: ApiUser
    {
        [DeserializeAs(Name ="id")]
        public override string Id { get; set; }

        [DeserializeAs(Name="name")]
        public override string Name { get; set; }

        [DeserializeAs(Name = "disabled")]
        public override bool Disabled { get; set; }

        [DeserializeAs(Name="site_admin")]
        public bool SiteAdmin { get; set; }

        [DeserializeAs(Name = "emails")]
        public override List<string> Emails { get; set; }

    }
}