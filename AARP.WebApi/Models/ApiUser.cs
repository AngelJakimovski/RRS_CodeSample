using System.Collections.Generic;

namespace AARP.WebApi.Models
{
    public abstract class ApiUser
    {
        public abstract string Id { get; set; }
        public abstract string Name { get; set; }
        public abstract bool Disabled { get; set; }
        public abstract List<string> Emails { get; set; }
    }
}
