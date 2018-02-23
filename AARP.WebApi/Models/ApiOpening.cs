using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.WebApi.Models
{
    public abstract class ApiJobOpening
    {
        public abstract int Id { set; get; }
        public abstract string Title { set; get; }
        public abstract string Description { set; get; }
        public abstract string Team { set; get; }
        public abstract string State { set; get; }
        public abstract string PositionType { set; get; }
        public abstract bool RemoteAvailable { set; get; }
        public abstract string HostedURL { set; get; }
        public abstract bool IsPrivate { set; get; }
        public abstract string ApplicationEmail { set; get; }
        public abstract bool IsArhived { set; get; }
        public abstract DateTime DateCreated { set; get; }
        public abstract DateTime DateModified { set; get; }
    }
}
