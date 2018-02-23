using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AARP.Services;

namespace AARP.Models
{
    public class AppConfigurationsViewModel
    {
        public AARP.Services.AppConfigs.CommonConfigs Commons { get; set; }

        public AARP.Services.AppConfigs.DistributeApplicantsConfigs DistributingApplicants { get; set; }

        public AARP.Services.AppConfigs.RefreshAssignedApplicantsStatusConfigs RefresingAssignedApplicants { get; set; }

        public AARP.Services.AppConfigs.RemindReviewersConfigs RemindingReviewers { get; set; }

        public AARP.Services.AppConfigs.EscalateTimeOutApplicantsConfigs EscalatingTimeOutApplicants { get; set; }

        public AARP.Services.AppConfigs.HandleReferralsConfigs HandlingReferrals { get; set; }

        public AARP.Services.AppConfigs.ReportErrorsConfig ReportingErrors { get; set; }

        public AppConfigurationsViewModel()
        {
            this.Commons = AppConfigsProvider.GetConfigs<AARP.Services.AppConfigs.CommonConfigs>();
            this.DistributingApplicants = AppConfigsProvider.GetConfigs<AARP.Services.AppConfigs.DistributeApplicantsConfigs>();
            this.RefresingAssignedApplicants = AppConfigsProvider.GetConfigs<AARP.Services.AppConfigs.RefreshAssignedApplicantsStatusConfigs>();
            this.RemindingReviewers = AppConfigsProvider.GetConfigs<AARP.Services.AppConfigs.RemindReviewersConfigs>();
            this.EscalatingTimeOutApplicants = AppConfigsProvider.GetConfigs<AARP.Services.AppConfigs.EscalateTimeOutApplicantsConfigs>();
            this.HandlingReferrals = AppConfigsProvider.GetConfigs<AARP.Services.AppConfigs.HandleReferralsConfigs>();
            this.ReportingErrors = AppConfigsProvider.GetConfigs<AARP.Services.AppConfigs.ReportErrorsConfig>();
        }

        public void SaveChanges()
        {
           AppConfigsProvider.SaveConfigs<AARP.Services.AppConfigs.CommonConfigs>(this.Commons);
           AppConfigsProvider.SaveConfigs<AARP.Services.AppConfigs.DistributeApplicantsConfigs>(this.DistributingApplicants);
           AppConfigsProvider.SaveConfigs<AARP.Services.AppConfigs.RefreshAssignedApplicantsStatusConfigs>(this.RefresingAssignedApplicants);
           AppConfigsProvider.SaveConfigs<AARP.Services.AppConfigs.RemindReviewersConfigs>(this.RemindingReviewers);
           AppConfigsProvider.SaveConfigs<AARP.Services.AppConfigs.EscalateTimeOutApplicantsConfigs>(this.EscalatingTimeOutApplicants);
           AppConfigsProvider.SaveConfigs<AARP.Services.AppConfigs.HandleReferralsConfigs>(this.HandlingReferrals);
           AppConfigsProvider.SaveConfigs<AARP.Services.AppConfigs.ReportErrorsConfig>(this.ReportingErrors);
        }
    }
}