using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.DAL
{
    public interface IRepository<T>
    {
        IList<T> List();

        T FindById(string id);

        void Add(T entity);

        void Update(T entity);

        void Delete(string id);
    }
}
