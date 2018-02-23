//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AARP.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class JobApplication
    {
        public int Id { get; set; }
        public string CandidateId { get; set; }
        public Nullable<System.DateTime> AppliedAt { get; set; }
        public Nullable<System.DateTime> RejectAt { get; set; }
        public Nullable<System.DateTime> LastActivity { get; set; }
        public string UrlCV { get; set; }
        public Nullable<int> ReviewerId { get; set; }
        public System.DateTime RecorededAt { get; set; }
        public string JobId { get; set; }
        public string ApplicationUrl { get; set; }
        public ApplicationReviewStatus ReviewStatus { get; set; }
        public string RejectionReason { get; set; }
        public int RemindCount { get; set; }
        public string CandidateName { get; set; }
        public string JobName { get; set; }
    }
}