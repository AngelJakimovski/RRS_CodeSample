using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AARP.Services;

namespace AARP.Tests
{
    [TestClass]
    public class BackgoundJobs_Test
    {
        [TestMethod]
        public void Test_GetNewAppliedApplications()
        {
            var results = BackgroundServices.GetNewAppliedApplications();
            Assert.AreNotEqual(0, results.Count);
            foreach (var item in results)
            {
                Console.WriteLine(item.CandidateId);
            }
        }
    }
}
