using System.Collections.Generic;

namespace AARP.WebApi.Helpers
{
    class RecruiterBoxData<T>
    {
        public RecruiterBoxMetaInfo Meta { get; set; }
        public List<T> Objects { get; set; } 
    }
}
