using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Infrashtructure.Error
{
    public class BaseAarpException : Exception
    {
        public BaseAarpException(string msg)
            : base(msg) 
        { }

        public BaseAarpException(string msg, params object[] args)
            : base(string.Format(msg, args)) 
        { }
    }
}
