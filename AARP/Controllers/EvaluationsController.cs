using AARP.Infrashtructure.DI;
using AARP.Logic;
using AARP.Models;
using AARP.Models.Evaluations;
using AARP.Models.Evaluations.Chart;
using AARP.Repositories;
using AARP.Services;
using AARP.Services.Extensions;
using AARP.WebApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace AARP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EvaluationsController : BaseController
    {
        IWebApiClient webApiClient = DiContainers.Global.Resolve<IWebApiClient>();

        private readonly IInterviewsRepository _interviewsRepository = DiContainers.Global.Resolve<IInterviewsRepository>(new { context = new AARPDbContext() });

        public EvaluationsController()
        {

        }

        public ActionResult Index(string order)
        {
            //var jobApplicantService = new JobApplicationProvider();
            //var reviewerResourceServuce = new ReviewerProvider();
            //var results = jobApplicantService.GetList().OrderByDescending(m => m.AppliedAt)
            //    .Select(applicant => new ApplicantViewModel()
            //    {
            //        Applicant = applicant,
            //        Reviewer = applicant.ReviewerId.HasValue ? reviewerResourceServuce.Get(applicant.ReviewerId.Value) : null
            //    });

            return View();
        }


        public ActionResult Stats()
        {
            var period = new DateRange()
            {
                dateFrom = DateTime.Now.AddDays(-30),
                dateTo = DateTime.Now
            };

            FillCharts(period);

            return View(EvaluationStatistics.GenerateStats(period));
        }

        public ActionResult GetStats(DateRange period)
        {
            ViewBag.DateFrom = period.dateFrom;
            ViewBag.DateTo = period.dateTo;

            FillCharts(period);

            return PartialView("Stats/_Fullpage", EvaluationStatistics.GenerateStats(period));
        }

        public ActionResult SelectInterviewers(DateTime? dateFrom, DateTime? dateTo)
        {
            using (var dbContext = new AARPDbContext())
            {
                // setting default values
                dateFrom = dateFrom ?? DateTime.MinValue;
                dateTo = dateTo ?? DateTime.Now;

                var interviewers = (from inter in dbContext.Interviewers
                                    join interw in dbContext.Interviews.Where(t => t.Date >= dateFrom && t.Date <= dateTo)
                                    on inter.Id equals interw.InterviewerId
                                    select new StatsInterviewersViewModel
                                    {
                                        Id = inter.Id,
                                        InterviewerName = inter.InterviewerName,
                                        InterviewerEmail = inter.InterviewerEmail,
                                        InterviewDate = interw.Date
                                    }).ToList();

                return PartialView("_InterviewersGrid", interviewers);
            }
        }

        #region Charts

        private void FillCharts(DateRange period)
        {
            ViewBag.MultiLineChart = GetMultiLineChartData();
            ViewBag.LineChart1 = EvaluationStatistics.GetInterviewsChartData(period,1);
            ViewBag.LineChart2 = EvaluationStatistics.GetInterviewsChartData(period, 2);
            ViewBag.StatsHeaderBarChartData = EvaluationStatistics.GetStatsHeaderBarChartData(period);
            

            ViewBag.OverallSeniority = EvaluationStatistics.GetSeniorityChartData(period);
            ViewBag.OverallStage = EvaluationStatistics.GetStageChartData(period);
            ViewBag.OverallLength = EvaluationStatistics.GetLengthChartData(period);
            ViewBag.OverallRating = EvaluationStatistics.GetRatingChartData(period);
        }

        private Models.Evaluations.Chart.Chart GetLineChartData()
        {
            Models.Evaluations.Chart.Chart _chart = new Models.Evaluations.Chart.Chart();
            _chart.labels = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "Novemeber", "December" };
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Current Year",
                data = new int[] { 28, 48, 40, 19, 86, 27, 90, 20, 45, 65, 34, 22 },
                borderColor = new string[] { "#800080" },
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return _chart;
        }

        private Models.Evaluations.Chart.Chart GetMultiLineChartData() 
        {
            Models.Evaluations.Chart.Chart _chart = new Models.Evaluations.Chart.Chart();
            _chart.labels = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "Novemeber", "December" };
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Current Year",
                data = new int[] { 28, 48, 40, 19, 86, 27, 90, 20, 45, 65, 34, 22 },
                borderColor = new string[] { "rgba(75,192,192,1)" },
                backgroundColor = new string[] { "rgba(75,192,192,0.4)" },
                borderWidth = "1"
            });
            _dataSet.Add(new Datasets()
            {
                label = "Last Year",
                data = new int[] { 65, 59, 80, 81, 56, 55, 40, 55, 66, 77, 88, 34 },
                borderColor = new string[] { "rgba(75,192,192,1)" },
                backgroundColor = new string[] { "rgba(75,192,192,0.4)" },
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return _chart;
        }
        #endregion

        public ActionResult Standings()
        {
            ViewBag.InterviewerStandings = EvaluationStandings.GetAllStandings();
            return View();
        }

        #region Interviews

        public ActionResult Interviews()
        {
            var filters = EvaluationInterviews.GetAllInterviewsFilters();
            var postFilters = new InterviewsFiltersPostViewModel();
            postFilters.selectedstages = filters.StageFilter.Keys.Select(x => x.Id).ToList();
            postFilters.selectedseniorities = filters.SeniorityFilter.Keys.Select(x => x.Id).ToList();
            var interviews = EvaluationInterviews.GetAllInterviewsList(postFilters);
            return View(interviews);
        }

        [HttpPost]
        public string Interviews(InterviewsFiltersPostViewModel filters)
        {
            string result = "";
            var interviews = EvaluationInterviews.GetAllInterviewsList(filters);
            int counter = 1;
            foreach (var interview in interviews.Interviews)
            {
                var ratings = "";

                for (var i = 0; i < 5; i++)
                {
                    if (interview.Rating > i)
                        ratings += string.Format(@"<i class=""glyphicon glyphicon-star""></i>");
                    else ratings += string.Format(@"<i class=""glyphicon glyphicon-star-empty""></i>");
                }
                result += string.Format(@"<tr><td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>     
                                        <td><a href=""Interviewee/{8}"">{3}</a></td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                        <td>{7}</td>
                                        </tr>", counter, interview.Interviewer, interview.InterviewDate, interview.Interviewee, interview.InterviewStage, interview.Seniority
                                                , interview.InterviewStatus, interview.Rating + " " + ratings, interview.IntervieweeID);

                counter++;
            }
            return result;
        }

        public PartialViewResult GetInterviewsFilter(int? interviewerId)
        {
            using (var context = new AARPDbContext())
            {
                ViewBag.Stages = context.InterviewStages.Select(s => new InterviewFilterItem
                {
                    Id = s.Id,
                    Value = s.Stage,
                    Count = context.Interviews.Count(c => c.InterviewerId == interviewerId && c.InterviewStageId == s.Id)
                }).ToList();

                ViewBag.Seniorities = context.Seniorities.Select(s => new InterviewFilterItem
                {
                    Id = s.Id,
                    Value = s.SeniorityName,
                    Count = context.Interviews.Count(c => c.InterviewerId == interviewerId && c.SeniorityId == s.Id)
                }).ToList();

                ViewBag.Technologies = context.InterviewTechnologies.Select(s => new InterviewFilterItem
                {
                    Id = s.Id,
                    Value = s.Technology,
                    Count = context.Interviews.Count(c => c.InterviewerId == interviewerId && c.TechnologyId == s.Id)
                }).ToList();

                ViewBag.TotalNumber = context.Interviews.Count(c => c.InterviewerId == interviewerId);

                var lastInterviews = (from i in context.Interviews.Where(w => w.InterviewerId == interviewerId).OrderByDescending(o => o.Date)
                                      join r in context.InterviewRatings on i.RatingId equals r.Id
                                      select new
                                      {
                                          Interview = i,
                                          Rating = r
                                      }).Take(10).ToList();

                ViewBag.PerformanceInfo = new InterviewerPerformanceViewModel()
                {
                    Name = context.Interviewers.FirstOrDefault(f => f.Id == interviewerId).InterviewerName,
                    AvgRating = lastInterviews.Count > 0 ? lastInterviews.Average(a => a.Rating.Rating) : 0,
                    Standings = lastInterviews.Count > 0 ? lastInterviews.GroupBy(g => g.Rating.Rating).Select(s => new StandingsInterviewRatingViewModel()
                    {
                        Rating = s.Key,
                        RatingCount = s.Count()
                    }).ToList() : null,
                    Attitude = new StandingsAttitudeViewModel()
                    {
                        Negative = lastInterviews.Count(t => t.Interview.InterviewerAttitude.Trim().Equals(DbConstants.AttitudeNegative)) / 10.0,
                        Neutral = lastInterviews.Count(t => t.Interview.InterviewerAttitude.Trim().Equals(DbConstants.AttitudeNeutral)) / 10.0,
                        Positive = lastInterviews.Count(t => t.Interview.InterviewerAttitude.Trim().Equals(DbConstants.AttitudePositive)) / 10.0,
                    }
                };
            }

            return PartialView("Interviews/_GeneralStats");
        }

        public JsonResult SelectInterviews(int? interviewerId, InterviewFilter filter, DateTime? from, DateTime? to, DataTableAjaxPostModel model)
        {
            string sortBy = "Date";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }


            int total;
            var interviews = _interviewsRepository.GetInterviews(interviewerId.Value, filter, out total, model.start, model.length, sortBy, sortDir);

            var dataItems = new DatatablesViewModel<InterviewRowItemViewModel>()
            {
                recordsTotal = total,
                recordsFiltered = total,
                data = interviews
            };

            return Json(dataItems);
        }
        #endregion

        #region Interviewee
        public ActionResult Interviewee(int id)
        {
            using (var dbContext = new AARPDbContext())
            {
                ViewBag.InterviewLengths = dbContext.InterviewLengths.ToList();
            }

            var profile = EvaluationInterviewee.GetIntervieweeProfile(id);

            return View(profile);
        }
        #endregion

        public ActionResult Interviewer(int id)
        {
            //dummy
            return HttpNotFound();
        }

        #region report

        public ActionResult Report()
        {
            return View();
        }

        public JsonResult SelectReportItems(DataTableAjaxPostModel model)
        {
            string sortBy = "Date";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            int total;

            var items = _interviewsRepository.GetReportItems(out total, model.start, model.length, sortBy, sortDir);

            var dataItems = new DatatablesViewModel<InterviewReportRowItemViewModel>()
            {
                recordsTotal = total,
                recordsFiltered = total,
                data = items
            };

            return Json(dataItems);
        }

        #endregion
        
        public ActionResult Admin()
        {
            var settings = EvaluationAdmin.GetAdminSettings();
            return View(settings);
        }

        [HttpPost]
        public ActionResult SaveSettings()
        {
            List<AdminSettingsViewModel> savedSettings = new List<AdminSettingsViewModel>();
            AdminSettingsViewModel techInterview1 = new AdminSettingsViewModel();
            techInterview1.Id = 1;
            techInterview1.EmailTimer = int.Parse(Request.Form["EmailTimer1"]);
            techInterview1.FeedbackMessage = Request.Form["FeedbackMessage1"];
            techInterview1.ReminderMessage= Request.Form["ReminderMessage1"];
            if (Request.Form["ReminderCheck1"]!=null)
            {
                techInterview1.EnableReminder = int.Parse(Request.Form["Reminder1"]);
            }
            else
            {
                techInterview1.EnableReminder = 0;
            }
            savedSettings.Add(techInterview1);

            AdminSettingsViewModel techInterview2 = new AdminSettingsViewModel();
            techInterview2.Id = 2;
            techInterview2.EmailTimer = int.Parse(Request.Form["EmailTimer2"]);
            techInterview2.FeedbackMessage = Request.Form["FeedbackMessage2"];
            techInterview2.ReminderMessage = Request.Form["ReminderMessage2"];
            if (Request.Form["ReminderCheck2"] != null)
            {
                techInterview2.EnableReminder = int.Parse(Request.Form["Reminder2"]);
            }
            else
            {
                techInterview2.EnableReminder = 0;
            }
            savedSettings.Add(techInterview2);

            if (EvaluationAdmin.SetAdminSettings(savedSettings))
            {
                MessageViewModel success = new MessageViewModel();
                success.Type = MessageType.Success;
                success.Content = "Settings Saved Sucessfully";
                success.Title = "Success";
                TempData["Message"] = success;
            }

            
            return RedirectToAction("Admin");
        }
        
        public ActionResult ConvertThisPageToPdf(string page)
        {
            // get the HTML code of this view

            // the base URL to resolve relative images and css
            String thisPageUrl = this.ControllerContext.HttpContext.Request.Url.AbsoluteUri;
            String baseUrl = thisPageUrl.Substring(0, thisPageUrl.Length - "Evaluations/ConvertThisPageToPdf".Length);

            //// instantiate the HiQPdf HTML to PDF converter
            //HtmlToPdf htmlToPdfConverter = new HtmlToPdf();

            //// hide the button in the created PDF
            //htmlToPdfConverter.HiddenHtmlElements = new string[] { "#convertThisPageButtonDiv" };

            //// render the HTML code as PDF in memory
            //byte[] pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(htmlToConvert, baseUrl);

            // send the PDF file to browser
            FileResult fileResult = new FileContentResult(new byte[0], "application/pdf");
            fileResult.FileDownloadName = "ThisMvcViewToPdf.pdf";

            return fileResult;
        }

        public JsonResult GetShortLink(string longUrl)
        {
            var shortUrl = UrlShortener.GetShortUrl(longUrl);
            return Json(shortUrl);
        }
    }
}