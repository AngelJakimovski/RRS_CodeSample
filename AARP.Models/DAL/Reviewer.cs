//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AARP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reviewer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int AssignedCount { get; set; }
        public int RejectCount { get; set; }
        public int AdvanceCount { get; set; }
        public Nullable<System.DateTime> RecentAssignedAt { get; set; }
        public string TimeZone { get; set; }
    }
}
