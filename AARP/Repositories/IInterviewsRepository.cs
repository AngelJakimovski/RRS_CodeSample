using System.Collections.Generic;
using AARP.Models.Evaluations;
using System;

namespace AARP.Repositories
{
    public interface IInterviewsRepository
    {
        List<InterviewRowItemViewModel> GetInterviews(int interviewerId, InterviewFilter filter, out int total, int start = 0, int size = 10, string sortBy="", bool sortDir = false);

        List<InterviewRowItemViewModel> GetInterviews(DateTime from, DateTime to, out int total, int start = 0, int size = 10);

        List<InterviewReportRowItemViewModel> GetReportItems(out int total, int start = 0, int size = 10, string sortBy = "", bool sortDir = false);
    }
}