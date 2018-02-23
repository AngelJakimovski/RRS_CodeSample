using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Infrashtructure.DI
{
    public static class DiContainers
    {
        private static WindsorContainer _global;

        /// <summary>
        /// Global container
        /// </summary>
        public static WindsorContainer Global
        {
            get
            {
                return _global ?? CreateGlobal();
            }
        }

        /// <summary>
        /// Initialize Global container
        /// </summary>
        /// <returns></returns>
        private static WindsorContainer CreateGlobal()
        {
            _global = new WindsorContainer();
            return _global;
        }
    }
}
