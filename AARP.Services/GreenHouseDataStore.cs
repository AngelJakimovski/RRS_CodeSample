using AARP.Infrashtructure.DI;
using Hangfire.Logging;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AARP.WebApi;
using AARP.WebApi.Models;

namespace AARP.Services
{
    public class GreenHouseDataStore
    {
        public static GreenHouseDataStore Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GreenHouseDataStore();
                return _instance;
            }
        }
        private static GreenHouseDataStore _instance;
        private static log4net.ILog _logger = LogManager.GetLogger(typeof(GreenHouseDataStore));

        private TimeSpan _freshnessThreshold = new TimeSpan(30, 0, 0);
        private object _lock = new object();
        private Timer _timer = null;

        private GreenHouseDataStore()
        {
            API = DiContainers.Global.Resolve<IWebApiClient>();

            Applicants = new List<ApiJobApplication>();
            Jobs = new List<ApiJob>();
            Candidates = new List<ApiCandidate>();

            Initialize();
        }

        public void Initialize()
        {
            var interval = 10000;//ms
            _timer = new Timer()
            {
                AutoReset = true,
                Interval = interval,
                Enabled = true
            };

            _timer.Elapsed += InvokeUpdateDataProcess;
        }

        public IWebApiClient API { get;  private set; }

        public List<ApiJobApplication> Applicants { get;  private set; }

        public List<ApiCandidate> Candidates { get; private set; }

        public List<ApiJob> Jobs { get; private set; }

        public DateTime? LastUpdateAt { get; private set; }

        public ApiJob GetJobById(string id)
        {
            return Jobs.FirstOrDefault(j => j.Id == id) ?? API.GetJobById(id);
        }

        public ApiJobApplication GetApplicationById(string id)
        {
            return /*Applicants.FirstOrDefault(a => a.Id.ToString() == id) ?? */API.GetApplicationById(id);
        }

        public void Refresh()
        {
            lock (_lock)
            {
                try
                {
                    var threshold = new TimeSpan(0, 2, 0);
                    if (LastUpdateAt.HasValue && DateTime.Now - LastUpdateAt.Value < threshold)
                        return;

                    _logger.Info("Refreshing by pulling new Greenhouse data");
                    Applicants = API.GetApplications().ToList();
                    Jobs = API.GetJobs().ToList();
                    Candidates = API.GetCandidates().ToList();
                }
                finally
                {
                    LastUpdateAt = DateTime.Now;
                    _logger.InfoFormat("GreenhouseDataStore has been updated at {0}", LastUpdateAt);
                }
            }
        }

        private bool NeedToSync()
        {
            return LastUpdateAt == null ||
                (DateTime.Now - LastUpdateAt.Value) >= _freshnessThreshold;
        }

        private void InvokeUpdateDataProcess(object sender, ElapsedEventArgs e)
        {
            if (NeedToSync())
                Refresh();
        }

        public override string ToString()
        {
            return string.Format("Jobs: {0}\nCandidates: {1}\nApplications: {2}",
                Jobs.Count,
                Candidates.Count,
                Applicants.Count);
        }
    }
}
