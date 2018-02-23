using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using AARP.GreenHouseAPI.Services;

namespace AARP.Tests
{
    [TestClass]
    public class GreenHouseAPI_Test
    {
        IGreenHouseAPI greenHouseAPI = new GreenHouseAPI.Services.GreenHouseAPI(CommonConfigurations.GreenHouseAPI_BaseUrl, CommonConfigurations.GreenHouseAPI_APIToken);
     
        [TestMethod]
        public void Test_GetUsers()
        {
            var results = greenHouseAPI.GetUsers();
            Assert.IsNotNull(results);
            Assert.AreNotEqual(results.Count, 0);
            Console.WriteLine("Users: {0}", results.Count);

            var testUser1 = results.FirstOrDefault(m=>m.Id == "45600");

            Assert.IsNotNull(testUser1);
            Assert.AreEqual(testUser1.Name, "Sy Nguyen Van");
            Assert.AreEqual(testUser1.Emails[0], "sy.n@scopicsoftware.com");
        }

        [TestMethod]
        public void Test_GetCandidates()
        {
            var results = greenHouseAPI.GetCandidates();

            Assert.IsNotNull(results);
            Assert.AreNotEqual(results.Count, 0);
            Console.WriteLine("Candidates: {0}", results.Count);

            var testCandidate = results.FirstOrDefault(m => m.Id == "3480565");
            Assert.IsNotNull(testCandidate);
            Assert.AreEqual(testCandidate.FirstName, "Yuri");
            Assert.AreNotEqual(testCandidate.Attachments.Count, 0);
            Assert.AreEqual(testCandidate.Attachments[0].FileName, "C#ASPNet - Юрий Чернов.doc");
            Assert.AreEqual(testCandidate.Coordinator.Name, "Alexander Svyatokho");
        }

        [TestMethod]
        public void Test_GetApplications()
        {
            var results = greenHouseAPI.GetApplications();

            Assert.IsNotNull(results);
            Assert.AreNotEqual(results.Count, 0);
            Console.WriteLine("Applications: {0}", results.Count);

        }

        [TestMethod]
        public void Test_GetJobs()
        {
            var results = greenHouseAPI.GetJobs();

            Assert.IsNotNull(results);
            Assert.AreNotEqual(results.Count, 0);
            Console.WriteLine("Jobs: {0}", results.Count);
        }

        [TestMethod]
        public void Test_GetStages(){
             var jobs = greenHouseAPI.GetJobs();

            Assert.IsNotNull(jobs);
            Assert.AreNotEqual(jobs.Count, 0);

            var stages = greenHouseAPI.GetApplicationStages(jobs.Select(m=>m.Id.ToString()).ToArray());
            Assert.IsNotNull(stages);
            Assert.AreNotEqual(0, stages.Count);
            Console.WriteLine("Stages: {0}",stages.Count);
        }
    }
}
