using AARP.Infrashtructure.Commons;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Infrashtructure.Logging
{
    public class Logger
    {
        private static readonly ILog log = LogManager.GetLogger("AARP");

        public static void Initialize()
        {
            string logConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                             Constants.LogFolder,
                                             "config.xml.log4net");
            log4net.Config.XmlConfigurator.Configure(new Uri(logConfigPath));
        }
    }
}
