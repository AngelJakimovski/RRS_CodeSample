using AARP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Repositories
{
    public abstract class Repository: IDisposable
    {
        protected AARPDbContext Context { get; set; }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Context != null) Context.Dispose();
            }
        }

    }
}