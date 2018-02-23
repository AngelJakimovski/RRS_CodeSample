using AARP.Models;
using AARP.Services.AppConfigs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services
{
    public static class AppConfigsProvider
    {
        public static TConfigs GetConfigs<TConfigs>() 
            where TConfigs : ConfigSection
        {
            Models.Configuration cnfgEntity = null;
            var key = typeof(TConfigs).Name;

            using (var dbContext = new AARPDbContext())
            {
                cnfgEntity = dbContext.Configurations.FirstOrDefault(cfg => cfg.Key == key);
            }

            if (cnfgEntity != null)
            {
                return JsonConvert.DeserializeObject<TConfigs>(cnfgEntity.Value);
            }
            else
                return Activator.CreateInstance<TConfigs>();
        }

        public static void SaveConfigs<TConfigs>(TConfigs configs)
        {
            var key = typeof(TConfigs).Name;
            var newConfigsValue = JsonConvert.SerializeObject(configs);

            using (var dbContext = new AARPDbContext())
            {
                var cnfgEntity = dbContext.Configurations.FirstOrDefault(cfg => cfg.Key == key);
                if (cnfgEntity != null)
                    cnfgEntity.Value = newConfigsValue;
                else
                    dbContext.Configurations.Add(new Configuration()
                    {
                        Key = key,
                        Value = newConfigsValue,
                    });
                dbContext.SaveChanges();
            }
        }

        public static AppConfigs.CommonConfigs CommonConfigs
        {
            get
            {
                return GetConfigs<CommonConfigs>();
            }
        }

        public static AppConfigs.DistributeApplicantsConfigs DistributeApplicantsConfigs
        {
            get
            {
                return GetConfigs<AppConfigs.DistributeApplicantsConfigs>();
            }
        }

        public static AppConfigs.RefreshAssignedApplicantsStatusConfigs RefreshApplicantsConfigs
        {
            get
            {
                return GetConfigs<AppConfigs.RefreshAssignedApplicantsStatusConfigs>();
            }
        }

        public static AppConfigs.RemindReviewersConfigs RemindReviewersConfigs
        {
            get
            {
                return GetConfigs<AppConfigs.RemindReviewersConfigs>();
            }
        }

        public static AppConfigs.HandleReferralsConfigs ReferralsHandlingConfigs
        {
            get
            {
                return GetConfigs<AppConfigs.HandleReferralsConfigs>();
            }
        }

        public static AppConfigs.EscalateTimeOutApplicantsConfigs EscalationConfigs
        {
            get
            {
                return GetConfigs<AppConfigs.EscalateTimeOutApplicantsConfigs>();
            }
        }

        public static AppConfigs.ReportErrorsConfig ErrorHandlingConfigs
        {
            get
            {
                return GetConfigs<AppConfigs.ReportErrorsConfig>();
            }
        }

        public static AppConfigs.SendEvaluationReport SendEvaluationReport
        {
            get
            {
                return GetConfigs<AppConfigs.SendEvaluationReport>();
            }
        }

        public static SyncEvaluations SyncEvaluations
        {
            get { return GetConfigs<SyncEvaluations>(); }
        }
    }
}
