using AARP.Models;
using AARP.Models.Evaluations;
using System.Linq;

namespace AARP.Logic
{
    public static class EvaluationInterviewee
    {
        public static IntervieweeProfileViewModel GetIntervieweeProfile(int intervieweeId)
        {
            var result = new IntervieweeProfileViewModel();

            using (var dbContext = new AARPDbContext())
            {
                var interviewee = dbContext.Interviewees.FirstOrDefault(x => x.Id == intervieweeId);
                if (interviewee != null)
                {
                    var opening = dbContext.JobPositions.FirstOrDefault(x => x.Id == interviewee.OpeningID);
                    var allInterviews = dbContext.Interviews.Where(x => x.IntervieweeId == intervieweeId).ToList();

                    result.IntervieweeName = interviewee.Name;
                    result.IntervieweeID = intervieweeId;
                    result.JobApplicationName = opening != null ? opening.Name : string.Empty;
                    if (allInterviews.Count > 0)
                    {
                        result.Interview1 = new IntervieweeResponseViewModel();
                        var interviewer1Id = allInterviews[0].InterviewerId;
                        result.Interview1.InterviewerName = dbContext.Interviewers.First(x => x.Id == interviewer1Id).InterviewerName;
                        result.Interview1.Survey = GenerateSurveyResult(allInterviews[0]);
                    }
                    if (allInterviews.Count > 1)
                    {
                        result.Interview2 = new IntervieweeResponseViewModel();
                        var interviewer2Id = allInterviews[1].InterviewerId;
                        result.Interview2.InterviewerName = dbContext.Interviewers.First(x => x.Id == interviewer2Id).InterviewerName;
                        result.Interview2.Survey = GenerateSurveyResult(allInterviews[1]);
                    }
                }


            }

            return result;
        }

        private static SurveyViewModel GenerateSurveyResult(Interview interview)
        {
            var result = new SurveyViewModel
            {
                InterviewerAttitude = interview.InterviewerAttitude?.Trim() ?? string.Empty,
                Id = interview.Id,
                InterviewLengthId = interview.InterviewLengthId,
                TestTask = interview.TestTask,
                TaskDifficultyLevel = interview.TaskDifficultyLevel?.Trim() ?? string.Empty,
                NoteFromInterviewee = interview.NoteFromInterviewee?.Trim() ?? string.Empty,
                RatingId = interview.RatingId
            };
            return result;
        }
    }
}