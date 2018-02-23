using AARP.Models;
using AutoMapper;
using log4net;
using System;
using System.Linq;
using AARP.WebApi.Commons;
using AARP.WebApi.Models;

namespace AARP.Services.Commands
{
    public class PullingNewApplicantsJob : ICommand
    {
        private static ILog _logger = LogManager.GetLogger(typeof(PullingNewApplicantsJob));
        private readonly object _applicantsSyncRoot = new object();
        public bool CanExecute()
        {
            return true;
        }

        public void Execute()
        {
            GreenHouseDataStore.Instance.Refresh();
            Mapper.CreateMap<ApiJobApplication, JobApplication>()
                .ForMember(dst => dst.ReviewStatus, opt => opt.UseValue(ReviewStatus.New))
                .ForMember(dst => dst.RecorededAt, opt => opt.UseValue(DateTime.UtcNow))
                .ForMember(dst => dst.RemindCount, opt => opt.UseValue(0))
                .ForMember(dst => dst.Source, opt => opt.MapFrom(src => src.Source.Name))
                .ForMember(dst => dst.CandidateName, opt => opt.MapFrom(src => ResolveCandidateName(src)))
                .ForMember(dst => dst.JobId, opt => opt.MapFrom(src => ResolveJobId(src)))
                .ForMember(dst => dst.JobName, opt => opt.MapFrom(src => ResolveJobName(src)));

            var openningJobs = GreenHouseDataStore.Instance.Jobs.Where(m => !string.IsNullOrEmpty(m.Status) && m.Status == JobStates.Open).ToArray();
            if (openningJobs == null || openningJobs.Length == 0)
            {
                _logger.InfoFormat("There is not openning job currently. GreenHouseDataStore has been updated at {0}.", GreenHouseDataStore.Instance.LastUpdateAt);
                return;
            }

            var applicantsService = new JobApplicationProvider();
            lock (_applicantsSyncRoot)
            {
                var newAplicants = GreenHouseDataStore.Instance.Applicants
                    .Where(ap => IsNewApplicant(ap, openningJobs)
                                 && applicantsService.Get(ap.Id) == null)
                    .Select(Mapper.Map<ApiJobApplication, JobApplication>)
                    .ToArray();

                applicantsService.AddRange(newAplicants);
            }

        }
        
        private string ResolveJobId(ApiJobApplication src)
        {
            var emptyId = "0";
            if (src == null || src.Jobs.FirstOrDefault() == null)
                return emptyId;
            //we must resolve id by RecourceUrl, because RecruiterBox doesn't give us application job id
            //var job = GreenHouseDataStore.Instance.Jobs.First(j => j.Name == src.Jobs.FirstOrDefault().Name);
            var srcJobResourceUrl = src.Jobs.FirstOrDefault()?.GetCustomFieldValue(JobCustomFields.ResourceUrl);
            if (string.IsNullOrEmpty(srcJobResourceUrl) || srcJobResourceUrl == ApiJob.UnknownCustomValue)
            {
                return emptyId;
            }
            var job = GreenHouseDataStore.Instance.Jobs.FirstOrDefault(j => j.GetCustomFieldValue(JobCustomFields.ResourceUrl) == srcJobResourceUrl);
            if (job == null)
            {
                return emptyId;
            }
            return job.Id;
        }

        private string ResolveJobName(ApiJobApplication src)
        {
            string unknown = "Unknown";
            if (src == null || src.Jobs.FirstOrDefault() == null)
                return unknown;

            //var job = GreenHouseDataStore.Instance.Jobs.First(j => j.Id == src.Jobs.FirstOrDefault().Id);
            //return job.Name;
            return src.Jobs.FirstOrDefault().Name;
        }

        private string ResolveCandidateName(ApiJobApplication src)
        {
            string unknown = "Unknown";
            if (src == null)
                return unknown;
            var candidate = GreenHouseDataStore.Instance.Candidates.FirstOrDefault(c => c.Id == src.CandidateId);
            return candidate!= null? candidate.GetFullName() : unknown;
        }

        private bool IsNewApplicant(ApiJobApplication applicant, ApiJob[] openingJobs)
        {
            /*var openingJobIds = openingJobs.Select(job => job.Id);
            return applicant != null && applicant.RejectedAt == null
                && applicant.CurrentStage != null && !string.IsNullOrEmpty(applicant.CurrentStage.Name) && applicant.CurrentStage.Name == ApplicationStages.ApplicationReviewCV
                && applicant.Jobs != null && applicant.Jobs.FirstOrDefault() != null && openingJobIds.Contains(applicant.Jobs.FirstOrDefault().Id);*/
            var openingJobsLinks = openingJobs.Select(job => job.GetCustomFieldValue(JobCustomFields.ResourceUrl));
            return applicant != null && applicant.RejectedAt == null
                   && applicant.CurrentStage != null && !string.IsNullOrEmpty(applicant.CurrentStage.Name) &&
                   applicant.CurrentStage.Name == ApplicationStages.ApplicationReviewCV
                   && applicant.Jobs != null && applicant.Jobs.FirstOrDefault() != null &&
                   applicant.Jobs.FirstOrDefault().GetCustomFieldValue(JobCustomFields.ResourceUrl) !=
                   ApiJob.UnknownCustomValue
                   &&
                   openingJobsLinks.Contains(
                       applicant.Jobs.FirstOrDefault().GetCustomFieldValue(JobCustomFields.ResourceUrl));
        }
    }
}
