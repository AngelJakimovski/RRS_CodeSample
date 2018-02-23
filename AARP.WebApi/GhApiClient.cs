using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AARP.Infrashtructure.DI;
using AARP.WebApi.Models;
using AARP.WebApi.Models.GreenHouse;
using log4net;
using RestSharp;
using RestSharp.Authenticators;
using AARP.WebApi.Models.RecruiterBox;

namespace AARP.WebApi
{
    [ImplementationFor(typeof(IWebApiClient))]
    public class GhApiClient : IWebApiClient
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(GhApiClient));

        private const string BaseUrl = GhApiConfig.BaseAddress;
        private const string TokenApi = GhApiConfig.TokenAPI;

        #region constructors
        private const int AttemptRequest = 10;
        public GhApiClient()
        {
        }
        #endregion
        
        protected T Execute<T>(RestRequest request) where T : new()
        {
            _logger.InfoFormat("{0}: {1}", request.Method, request.Resource);

            var client = new RestClient
            {
                BaseUrl = new Uri(BaseUrl),
                Authenticator = new HttpBasicAuthenticator(TokenApi, string.Empty)
            };

            var response = client.Execute<T>(request);

            //Sleep for 1 min if only 1 RateLimit remaining
            var rateLimitRemaining = response.Headers.FirstOrDefault(m => string.Equals(m.Name, "X-Ratelimit-Remaining"));

            if (rateLimitRemaining != null && int.Parse(rateLimitRemaining.Value.ToString()) == 1)
            {
                System.Diagnostics.Trace.WriteLine("GreenHouse API has reached Rate-Limit");
                System.Threading.Thread.Sleep(new TimeSpan(0, 1, 0));
            }

            if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
            {
                System.Threading.Thread.Sleep(10000);
                return Execute<T>(request);
            }

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            if (response.Data == null)
            {
                return new T();
            }

            _logger.InfoFormat("[DONE] {0}: {1}", request.Method, request.Resource);
            return response.Data;
        }

        protected Task<T> ExecuteAsync<T>(RestRequest request) where T : new()
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            var client = new RestClient
            {
                BaseUrl = new Uri(BaseUrl),
                Authenticator = new HttpBasicAuthenticator(TokenApi, string.Empty)
            };
            client.ExecuteAsync<T>(request, response =>
            {
                var rateLimitRemaining = response.Headers.FirstOrDefault(m => string.Equals(m.Name, "X-Ratelimit-Remaining"));
                if (rateLimitRemaining != null && int.Parse(rateLimitRemaining.Value.ToString()) == 1)
                {
                    _logger.InfoFormat("GreenHouse API has reached Rate-Limit. Sleep for 1 min.");
                    System.Threading.Thread.Sleep(new TimeSpan(0, 1, 0));
                }

                if (response.ErrorException != null)
                {
                    _logger.Error("Error while making request to Greenhouse haverst api", response.ErrorException);
                    throw response.ErrorException;
                }
                taskCompletionSource.SetResult(response.Data);
            });
            return taskCompletionSource.Task;
        }

        protected virtual RestRequest CreateRequestForPaging(string url, int currentPage, int pageSize)
        {
            return new RestRequest($"{url}?page={currentPage}&per_page={pageSize}", Method.GET);
        }

        protected IList<T> GetAll<T>(string url)
        {

            var results = new List<T>();
            int page = 1;
            while (true)
            {
                List<T> currentPageResults = new List<T>();
                var maxPageSize = 500;
                var maxAttempt = 5;
                var request = CreateRequestForPaging(url, page, maxPageSize);

                request.RequestFormat = DataFormat.Json;
                currentPageResults = TryExecute<List<T>>(request, maxAttempt) ?? new List<T>();

                if (currentPageResults == null || currentPageResults.Count == 0)
                    break;
                if (currentPageResults.Count == 1)
                    break;
                results.AddRange(currentPageResults);
                page++;
            }

            _logger.InfoFormat("GET {0} entites of {1}", results.Count, typeof(T).Name);
            return results;
        }

        protected async Task<IList<T>> GetAllAsync<T>(string url)
        {
            var results = new List<T>();
            int page = 1;
            while (true)
            {
                var maxPageSize = 500;
                var request = new RestRequest(string.Format("{0}?page={1}&per_page={2}", url, page, maxPageSize), Method.GET);
                var currentPageResults = await ExecuteAsync<List<T>>(request);
                if (currentPageResults == null || currentPageResults.Count == 0)
                    break;
                if (currentPageResults.Count == 1)
                    break;
                results.AddRange(currentPageResults);
                page++;
            }
            return results;
        }

        protected T TryExecute<T>(RestRequest request, int maxAttempt = 0) where T : new()
        {
            var attemptCount = 0;
            T results = default(T);

            while (attemptCount <= maxAttempt)
            {
                try
                {
                    results = Execute<T>(request);
                    break;
                }
                catch
                {
                    attemptCount++;
                }
            }

            return results;
        }

        #region APIs
        public IList<ApiCandidate> GetCandidates()
        {
            return GetAll<GhCandidate>(GhApiConfig.GetCandidatesEndpoint).ToList<ApiCandidate>();
        }

        public IList<ApiJobApplication> GetApplications()
        {
            return GetAll<GhJobApplication>(GhApiConfig.GetJobApplicationsEndpoints).ToList<ApiJobApplication>();
        }
        
        public IList<ApiUser> GetUsers()
        {
            return GetAll<GhGreenHouseUser>(GhApiConfig.GetUsersEndpoint).ToList<ApiUser>();
        }

        public IList<ApiApplicationStage> GetApplicationStages(params string[] jobIds)
        {
            var results = new List<GhApplicationStage>();
            foreach(var jobId in jobIds)
            {
                var stagesOfJob = GetAll<GhApplicationStage>(GhApiConfig.GetApplicationStagesEndpoint(jobId));
                foreach (var stage in stagesOfJob) 
                    stage.JobId = jobId;

                results.AddRange(stagesOfJob);
            }
            return results.ToList<ApiApplicationStage>();
        }

        public IList<ApiJob> GetJobs()
        {
            return GetAll<GhJob>(GhApiConfig.GetJobs).ToList<ApiJob>();
        }

        public ApiJobApplication GetApplicationById(string id)
        {
            var request = new RestRequest(GhApiConfig.GetApplicationByIdEndpoint(id), Method.GET);
            return Execute<GhJobApplication>(request);
        }

        public ApiJob GetJobById(string id)
        {
            var request = new RestRequest(GhApiConfig.GetJobByIdEndpoint(id), Method.GET);
            return Execute<GhJob>(request);
        }

        public IList<ApiInterview> GetInterviews()
        {
            throw new NotImplementedException();
        }
        public IList<RecruiterBoxEvaluation> GetInterviewers()
        {
            throw new NotImplementedException();
        }
        public IList<ApiCandidate> GetInterviewees()
        {
            throw new NotImplementedException();
        }
        public IList<ApiJobOpening> GetJobTitles()
        {
            throw new NotImplementedException();
        }

        public void CleanUp()
        {
            
        }

        #endregion
    }
}