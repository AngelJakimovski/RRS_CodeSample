using System;
using System.Collections.Generic;
using RestSharp.Deserializers;

namespace AARP.WebApi.Models.GreenHouse
{
    internal sealed class GhCandidate: ApiCandidate
    {
        [DeserializeAs(Name = "id")]
        public override string Id { get; set; }

        [DeserializeAs(Name = "first_name")]
        public override string FirstName { get; set; }

        [DeserializeAs(Name = "last_name")]
        public override string LastName { get; set; }

        [DeserializeAs(Name = "company")]
        public string Company { get; set; }

        [DeserializeAs(Name = "title")]
        public string Title { get; set; }

        [DeserializeAs(Name = "created_at")]
        public DateTime? CreatedAt { get; set; }

        [DeserializeAs(Name = "last_activity")]
        public DateTime? LastActivity { get; set; }

        [DeserializeAs(Name = "photo_url")]
        public string PhotoUrl { get; set; }

        [DeserializeAs(Name = "attachments")]
        public List<GhCandidateAttachment> Attachments { get; set; }

        [DeserializeAs(Name = "application_ids")]
        public List<string> ApplicationIds { get; set; }

        [DeserializeAs(Name = "phone_numbers")]
        public List<GhCandidatePhoneNumber> PhoneNumbers { get; set; }

        [DeserializeAs(Name = "addresses")]
        public List<GhCandidateAddress> Addresses { get; set; }

        [DeserializeAs(Name = "email_addresses")]
        public List<GhCandidateEmail> Emails { get; set; }

        [DeserializeAs(Name = "website_addresses")]
        public List<string> WebsiteAddresses { get; set; }

        [DeserializeAs(Name = "social_media_addresses")]
        public List<string> SocialMediaAddresses { get; set; }

        [DeserializeAs(Name = "recruiter")]
        public GhGreenHouseUser Recruiter { get; set; }

        [DeserializeAs(Name = "coordinator")]
        public GhGreenHouseUser Coordinator { get; set; }

        [DeserializeAs(Name = "tags")]
        public List<string> Tags { get; set; }

        [DeserializeAs(Name = "custom_fields")]
        public Dictionary<string, string> CustomFields { get; set; }

        public override string Email { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override int OpeningID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override int StageID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string StageName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string State { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
