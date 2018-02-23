using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.WebApi.Models
{
    public abstract class ApiUser2
    {
        public abstract string Id { get; set; }
        public abstract string Name { get; set; }
        public abstract string Email { get; set; }
    }
}
