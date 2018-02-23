using AARP.Models;
using Postal;
using AARP.WebApi.Models;

namespace AARP.Services.Emails
{
    public class NotificationOfNewReferralEmail : Email
    {
        public JobApplication Applicant { get; set; }

        public ApiJob Job { get; set; }

        public string SendTo { get; set; }
    }
}
