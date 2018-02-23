using System;
using System.Collections.Generic;

namespace AARP.WebApi.Models
{
    public abstract class ApiJobApplication
    {
        public abstract int Id { get; set; }
        public abstract string CandidateId { get; set; }
        public abstract bool IsProspect { get; set; }
        public abstract DateTime? AppliedAt { get; set; }
        public abstract DateTime? RejectedAt { get; set; }
        public abstract DateTime? AcceptedAt { get; set; }
        public abstract ApiApplicationSource Source { get; set; }
        public abstract string RejectionReason { get; set; }
        public abstract List<ApiJob> Jobs { get; set; }
        public abstract ApiApplicationStage CurrentStage { get; set; }
    }
}
