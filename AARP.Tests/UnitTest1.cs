using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AARP.Services.Commands;
using AARP.Services;
using AARP.Models;
using AARP.WebApi.Models.RecruiterBox;

namespace AARP.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var reviewerProvider = new ReviewerProvider();
            var workingDays = new DayOfWeek[]{ DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday};
            var fromTime = new TimeSpan(0,0, 0);
            var toTime = new TimeSpan(24, 0, 0);
            var allReviwers = reviewerProvider.GetList();
            foreach (var rvr in allReviwers)
            {
                var isInWorkingHours = rvr.IsInWorkingHours(fromTime, toTime, workingDays);
                Console.WriteLine("{0}: {1}", rvr.Name, isInWorkingHours);
            }
        }
    }
}
