namespace AARP.WebApi
{
    internal static class GhApiConfig
    {
        public const string BaseAddress = "https://harvest.greenhouse.io/v1/";
        public const string GetUsersEndpoint = "users";
        public const string GetCandidatesEndpoint = "candidates";
        public const string GetJobApplicationsEndpoints = "applications";
        public const string GetInterviewersEndpoint = "interviewers";
        public const string GetJobs = "jobs";
        public const string TokenAPI = "84a0a1017ef5d00dd9e8cb252e6153de";

        public static string GetApplicationStagesEndpoint(string jobId)
        {
            return string.Format("jobs/{0}/stages", jobId);
        }

        public static string GetApplicationByIdEndpoint(string id)
        {
            return GetJobApplicationsEndpoints + "/" + id;
        }

        internal static string GetJobByIdEndpoint(string id)
        {
            return GetJobs + "/" + id;
        }

    }
}