using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Models
{
    public class ReviewerViewModel
    {
        public Reviewer Reviewer { get; set; }

        [DisplayName("Average Hours")]
        public double? AvgTime { get; set; }
    }
}
