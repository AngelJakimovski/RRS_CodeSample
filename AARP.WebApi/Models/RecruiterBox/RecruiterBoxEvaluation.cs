using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.WebApi.Models.RecruiterBox
{
    public class RecruiterBoxEvaluation : ApiEvaluation
    {
        [JsonProperty(PropertyName = "id")]
        public override int Id { set; get; }

        [JsonProperty(PropertyName = "feedback")]
        public List<RecruiterBoxEvaluationFeedback> Feedback { set; get; }
    }
}
