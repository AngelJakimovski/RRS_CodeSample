using AARP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.WebApi.Models
{
    public abstract class ApiEvaluationFeedback
    {
        public int Id { set; get; }

        public abstract int Rating { set; get; }
        
        public abstract string FeedbackText { set; get; }
        public abstract string Criteria { set; get; }

        public abstract DateTime DateSubmitted { set; get; }
        
        
    }
}
