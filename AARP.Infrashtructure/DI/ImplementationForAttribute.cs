using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Infrashtructure.DI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ImplementationForAttribute : Attribute
    {
        public Type[] ComponentTypes { get; set; }

        public ImplementationForAttribute(params Type[] componentTypes)
        {
            ComponentTypes = componentTypes;
        }
    }
}
