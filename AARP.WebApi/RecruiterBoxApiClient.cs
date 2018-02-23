using System;
using System.Collections.Generic;
using System.Linq;
using AARP.WebApi.Helpers;
using AARP.WebApi.Models;
using AARP.WebApi.Models.RecruiterBox;

namespace AARP.WebApi
{
    public class RecruiterBoxApiClient : IWebApiClient
    {
        private readonly IWebDriverApi _webDriverApi;

        #region public RecruiterBox's api info
        private const string RecruiterBoxApiUrl = "https://api.recruiterbox.com/v1/";
        private const string RecruiterBoxToken = "11d5259ecebb4be68859fec082ec05bd";

        private const string OpeningsPublicEndpoint = "openings/";
        #endregion

        private const string OpeningsEndpoint = "api/v1/openings/";
        private const string CandidatesEndpoint = "api/v1/candidates/";
        private const string UsersEndpoint = "api/v1/users/";
        private const string JobStagesEndpoint = "api/v1/opening_stages/";
        private const string CandidateStageMovements = "api/v1/stage_movements/?candidate=";

        #region RecruiterBox's api v2
        private const string InterviewsEndpoint = "interviews";
        private const string InterviewersEndpoint = "evaluations";
        private const string IntervieweesEndpoint = "candidates";
        private const string JobsEndpoint = "openings";
        #endregion

        public RecruiterBoxApiClient(IWebDriverApi webDriverApi)
        {
            _webDriverApi = webDriverApi;
        }
        

        public IList<ApiCandidate> GetCandidates()
        {
            var data = _webDriverApi.LoadDataList<RecruiterBoxCandidate>($"{CandidatesEndpoint}?limit=50000");
            return data != null ? data.ToList<ApiCandidate>() : new List<ApiCandidate>();
        }

        public IList<ApiJobApplication> GetApplications()
        {
            var data = _webDriverApi.LoadDataList<RecruiterBoxJobApplication>($"{CandidatesEndpoint}?limit=50000");
            return data != null ? data.ToList<ApiJobApplication>() : new List<ApiJobApplication>();
        }

        public ApiJobApplication GetApplicationById(string id)
        {
            var jobApplication = _webDriverApi.LoadDataObject<RecruiterBoxJobApplication>($"{CandidatesEndpoint}{id}/");
            if (jobApplication!=null)
            jobApplication.StageMovements =
                _webDriverApi.LoadDataList<RecruiterBoxStageMovement>($"{CandidateStageMovements}{id}");
            return jobApplication;
        }

        public IList<ApiUser> GetUsers()
        {
            return _webDriverApi.LoadDataList<RecruiterBoxUser>($"{UsersEndpoint }?limit = 50000").ToList<ApiUser>();
        }

        public IList<ApiApplicationStage> GetApplicationStages(params string[] jobIds)
        {
            return
                _webDriverApi.LoadDataList<RecruiterBoxApplicationStage>(JobStagesEndpoint).ToList<ApiApplicationStage>();
        }

        public IList<ApiJob> GetJobs()
        {
            var data = _webDriverApi.LoadDataList<RecruiterBoxJob>(OpeningsEndpoint);
            return data != null ? data.ToList<ApiJob>() : new List<ApiJob>();
        }

        public ApiJob GetJobById(string id)
        {
            try
            {
                return _webDriverApi.LoadDataObject<RecruiterBoxJob>($"{OpeningsEndpoint}{id}/");
            }
            catch (NullReferenceException)
            {
                return new RecruiterBoxJob(){Id = id};
            }
        }

        public IList<ApiInterview> GetInterviews()
        {
            return _webDriverApi.LoadDataList_New<RecruiterBoxInterview>($"{InterviewsEndpoint}?limit=50000").ToList<ApiInterview>();
        }
        public IList<RecruiterBoxEvaluation> GetInterviewers()
        {
            return _webDriverApi.LoadDataList_New<RecruiterBoxEvaluation>($"{InterviewersEndpoint}?limit=50000").ToList<RecruiterBoxEvaluation>();
        }
        public IList<ApiCandidate> GetInterviewees()
        {
            return _webDriverApi.LoadDataList_New<RecruiterBoxCandidate>($"{IntervieweesEndpoint}?limit=50000").ToList<ApiCandidate>();
        }
        public IList<ApiJobOpening> GetJobTitles()
        {
            return _webDriverApi.LoadDataList_New<RecruiterBoxOpening>($"{JobsEndpoint}?limit=50000").ToList<ApiJobOpening>();
        }

        public void CleanUp()
        {
            _webDriverApi.CleanUp();
        }
        
    }
}