using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AARP.GreenHouseAPI.Services;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AARP.Tests
{
    [TestClass]
    public class GreenHouseData_Test
    {
        static IGreenHouseAPI greenHouseAPI = new GreenHouseAPI.Services.GreenHouseAPI(CommonConfigurations.GreenHouseAPI_BaseUrl, CommonConfigurations.GreenHouseAPI_APIToken);

        [TestMethod]
        public void Test_RefreshGreenHouse()
        {
            IGreenHouseData greenHouseData = new GreenHouseData(greenHouseAPI);
            greenHouseData.RefreshData();
            var dataStr = greenHouseData.ToString();

            Console.WriteLine(dataStr);
        }
    }
}
