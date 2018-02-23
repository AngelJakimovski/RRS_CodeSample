using AARP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.WebApi.Models
{
    public abstract class ApiInterview : IAARPEntity
    {
        public abstract int Id { get; set; }

        public abstract string Title { get; set; }
        public abstract string Description { get; set; }

        public abstract DateTime Time { get; set; }

        public abstract int Duration { get; set; }

        public abstract string Location { get; set; }

        public abstract object Type { get; set; }

        public abstract Dictionary<string,int> Candidate { get; set; }

        public abstract DateTime DateCreated { get; set; }

        public abstract bool IsPrivate { get; set; }

        public abstract bool IsCancelled { get; set; }

        public abstract List<Dictionary<string,string>> Invitees { get; set; }
        
    }
}
