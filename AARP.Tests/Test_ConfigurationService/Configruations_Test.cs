using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AARP.Services;
using AARP.Services.AppConfigs;

namespace AARP.Tests.Test_ConfigurationService
{
    [TestClass]
    public class Configruations_Test
    {
        [TestMethod]
        public void Test_Applicants_Distribution_Config()
        {
          
            DistributeApplicantsConfigs config = new DistributeApplicantsConfigs()
            {
                IsEnabled = true,
                Recurrence = "* * * * *"
            };
            AppConfigsProvider.SaveConfigs(config);

            var savedConfigs = AppConfigsProvider.GetConfigs<DistributeApplicantsConfigs>();
            Assert.AreEqual(config.ToString(), savedConfigs.ToString(), "Get and Save configuration inproperly");
            Console.WriteLine(savedConfigs);
        }

        [TestMethod]
        public void Test_Update_Assigned_Applicants_Status_Configs()
        {
                RefreshAssignedApplicantsStatusConfigs config = new RefreshAssignedApplicantsStatusConfigs()
                {
                    IsEnabled = false,
                    Recurrence = "* * * * *"
                };

                AppConfigsProvider.SaveConfigs(config);

                var savedConfigs = AppConfigsProvider.GetConfigs<RefreshAssignedApplicantsStatusConfigs>();
                Assert.AreEqual(config.ToString(), savedConfigs.ToString(), "Get and Save configuration inproperly");
                Console.WriteLine(savedConfigs);
        }

        [TestMethod]
        public void Test_Escalating_Unhandled_Applicants_Configs()
        {
             EscalateTimeOutApplicantsConfigs config = new EscalateTimeOutApplicantsConfigs()
                {
                    IsEnabled = true,
                    RemindTheshold = 3,
                    ReportToEmails = "vulh91@gmail.com, vu.l@scopicosftware.com, angel.j@scopicsoftware.com, hai.v@scopicsoftware.com"
                };

                AppConfigsProvider.SaveConfigs(config);

                var savedConfigs = AppConfigsProvider.GetConfigs<EscalateTimeOutApplicantsConfigs>();
                Assert.AreEqual(config.ToString(), savedConfigs.ToString(), "Get and Save configuration inproperly");
                Console.WriteLine(savedConfigs);
        }
    }
}
