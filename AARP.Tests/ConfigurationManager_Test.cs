using AARP.DAL;
using AARP.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Tests
{
    [TestClass]
    public class ConfigurationManager_Test
    {
        IRepositoryFactory repositoryFactory
        {
            get { return RepositoryFactory.Instance; }
        }
        IConfigurationManager configManager
        {
            get { return new AppConfigurationManager(repositoryFactory); }
        }


        [TestMethod]
        public void Test_SaveChangesCommonConfig()
        {
            //var vm = new AppConfigurationsViewModel(repositoryFactory, configManager);
            //Console.WriteLine("Escalation email: {0}", vm.EscalationEmail);
            //var newEmail = string.Format("{0}@scopicsoftware.com", Guid.NewGuid().ToString());

            //vm.EscalationEmail = newEmail;

            //Assert.AreEqual(newEmail, vm.EscalationEmail );
        }
    }
}
