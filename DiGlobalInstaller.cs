using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AARP.WebApi;

namespace AARP
{
    public class DiGlobalInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            //container.Register(Component.For<IWebApiClient>().ImplementedBy<GhApiClient>().LifestyleSingleton());
            container.Register(
                Component.For<IWebApiClient>()
                    .ImplementedBy<RecruiterBoxApiClient>()
                    .LifestyleSingleton()
                    .DependsOn((kernel, dependencies) => dependencies["webDriverApi"] = RecruiterBoxPhantomJsApi.Instance));
        }
    }
}
