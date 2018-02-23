using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp.Deserializers;

namespace AARP.WebApi.Models.GreenHouse
{
    internal sealed class GhJobApplication: ApiJobApplication
    {
        private List<ApiJob> _jobs;
        private List<GhJob> _ghJobs;

        public GhJobApplication()
        {
            _jobs = new List<ApiJob>();
            _ghJobs = new List<GhJob>();
        }

        [DeserializeAs(Name = "id")]
        public override int Id { get; set; }

        [DeserializeAs(Name = "name")]
        public string Name { get; set; }

        [DeserializeAs(Name = "candidate_id")]
        public override string CandidateId { get; set; }

        [DeserializeAs(Name = "person_id")]
        public string PersonId { get; set; }

        [DeserializeAs(Name = "prospect")]
        public override bool IsProspect { get; set;}

        [DeserializeAs(Name = "applied_at")]
        public override DateTime? AppliedAt { get; set; }

        [DeserializeAs(Name = "rejected_at")]
        public override DateTime? RejectedAt { get; set; }

        //ToDo if this attribute will be used, need to initialize it
        public override DateTime? AcceptedAt
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        [DeserializeAs(Name = "last_activity_at")]
        public DateTime? LastActivityAt { get; set; }

        [DeserializeAs(Name = "source_not_deserialize")]
        public override ApiApplicationSource Source
        {
            get { return GhSource; }
            set { GhSource = value as GhApplicationSource; }
        }
        [DeserializeAs(Name = "source")]
        public GhApplicationSource GhSource { get; set; }

        [DeserializeAs(Name = "credited_to")]
        public GhGreenHouseUser CreatedTo { get; set; }

        [DeserializeAs(Name = "rejection_reason")]
        public override string RejectionReason { get; set; }
        
        [DeserializeAs(Name = "jobs_not_deserialize")]
        public override List<ApiJob> Jobs
        {
            get { return _jobs; }
            set { _jobs = value; }
        }

        [DeserializeAs(Name = "jobs")]
        public List<GhJob> GhJobs
        {
            get { return _ghJobs; }
            set
            {
                _ghJobs = value;
                _jobs = _ghJobs.ToList<ApiJob>();
            }
        }

        [DeserializeAs(Name = "status")]
        public string Status { get; set; }

        [DeserializeAs(Name = "current_stage_not_deserialize")]
        public override ApiApplicationStage CurrentStage
        {
            get { return GhCurrentStage; }
            set { GhCurrentStage = value as GhApplicationStage; }
        }
        [DeserializeAs(Name = "current_stage")]
        public GhApplicationStage GhCurrentStage { get; set; }

    }
}