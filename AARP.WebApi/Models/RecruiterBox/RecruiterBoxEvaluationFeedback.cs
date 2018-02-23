using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.WebApi.Models.RecruiterBox
{
    public sealed class RecruiterBoxEvaluationFeedback : ApiEvaluationFeedback
    {
        [JsonProperty(PropertyName = "rating")]
        public override int Rating { set; get; }

        [JsonProperty(PropertyName = "feedback_text")]
        public override string FeedbackText { set; get; }

        [JsonProperty(PropertyName = "criteria")]
        public override string Criteria { set; get; }

        [JsonProperty(PropertyName = "submitted_by")]
        public RecruiterBoxUser SubmittedBy { set; get; }

        [JsonProperty(PropertyName = "date_submitted")]
        public override DateTime DateSubmitted{ set; get; }

        
    }
}
