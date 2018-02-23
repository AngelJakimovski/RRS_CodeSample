using System;
using System.Collections.Generic;
using AARP.WebApi.Commons;
using AARP.WebApi.Models.GreenHouse;
using Newtonsoft.Json;

namespace AARP.WebApi.Models.RecruiterBox
{
    using System.Linq;

    internal sealed class RecruiterBoxJobApplication: ApiJobApplication
    {
        private const string RejectedStateName = "/api/v1/candidate_states/4/";

        [JsonProperty(PropertyName = "id")]
        public override int Id { get; set; }
        public override string CandidateId {
            get { return Id.ToString(); }
            set { }
        }
        [JsonProperty(PropertyName = "is_prospect")]
        public override bool IsProspect { get; set; }
        [JsonProperty(PropertyName = "created_on")]
        public override DateTime? AppliedAt { get; set; }

        public override DateTime? RejectedAt
        {
            get { return IsRejected ? StateMetadata?.LastUpdateDate ?? (DateTime?) DateTime.UtcNow : null; }
            set { }
        }

        public override DateTime? AcceptedAt
        {
            get
            {
                if (CurrentStage?.Name == ApplicationStages.ApplicationReviewCV)
                {
                    return null;
                }
                var acceptedMovements =
                    StageMovements?.Where(
                        stageMovement =>
                            !stageMovement.IsDeleted &&
                            stageMovement.Position != 0 &&
                            stageMovement.JobLink == JobLink);
                if (acceptedMovements != null)
                {
                    return acceptedMovements.Min(stageMovement => stageMovement.CreationDate);
                }
                return DateTime.UtcNow;
            }
            set { throw new Exception("Unable to set AcceptedAt property for RecruiterBoxApplication. This property is read-only");}
        }

        public IList<RecruiterBoxStageMovement> StageMovements { get; set; } 

        public override ApiApplicationSource Source
        {
            get
            {
                return new ApiApplicationSource() {Name = IsReferral ? ApplicantSources.Referral : SourceData?.Channel};
            }
            set { }
        }
        
        public override string RejectionReason
        {
            get { return IsRejected ? StateMetadata?.Comment : string.Empty; }
            set { }
        }

        public override List<ApiJob> Jobs
        {
            get
            {
                return new List<ApiJob>()
                {
                    new RecruiterBoxJob()
                    {
                        Name = JobName,
                        ResourceUri = JobLink
                    }
                };
            }
            set { }
        }

        public override ApiApplicationStage CurrentStage
        {
            get { return new GhApplicationStage() {Name = StageName}; }
            set { }
        }

        [JsonProperty(PropertyName = "opening_name")]
        public string JobName { get; set; }
        [JsonProperty(PropertyName = "assigned_opening")]
        public string JobLink { get; set; }
        [JsonProperty(PropertyName = "stage_name")]
        public string StageName { get; set; }
        [JsonProperty(PropertyName = "current_stage")]
        public string StageLink { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "source_data")]
        public RecruiterBoxCandidateSource SourceData { get; set; }

        [JsonProperty(PropertyName = "state_metadata")]
        public RecruiterBoxApplicationStateMetadata StateMetadata { get; set; }

        public bool IsRejected => State == RejectedStateName;

        public bool IsReferral
            =>
                SourceData != null && SourceData.Channel == "Upload" && !string.IsNullOrEmpty(SourceData.Source) &&
                SourceData.Source.ToLower().Contains("referral");
    }
}
