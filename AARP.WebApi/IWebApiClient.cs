using System.Collections.Generic;
using AARP.WebApi.Models;
using AARP.WebApi.Models.RecruiterBox;

namespace AARP.WebApi
{
    public interface IWebApiClient
    {

        IList<ApiCandidate> GetCandidates();

        IList<ApiJobApplication> GetApplications();

        ApiJobApplication GetApplicationById(string id);
        
        IList<ApiUser> GetUsers();

        IList<ApiApplicationStage> GetApplicationStages(params string[] jobIds);

        IList<ApiJob> GetJobs();

        ApiJob GetJobById(string id);

        IList<ApiInterview> GetInterviews();
        IList<RecruiterBoxEvaluation> GetInterviewers();
        IList<ApiCandidate> GetInterviewees();
        IList<ApiJobOpening> GetJobTitles();

        void CleanUp();
    }
}
