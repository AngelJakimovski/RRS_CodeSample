using AARP.Models;
using Postal;
using AARP.WebApi.Models;

namespace AARP.Services.Emails
{
    public class ReviewApplicationEmail : Postal.Email
    {
        public JobApplication JobApplication { get; set; }
        public Reviewer Reviewer { get; set; }
        public ApiJob Job { get; set; }
    }
}