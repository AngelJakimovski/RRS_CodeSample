using AARP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services
{
    public interface IResourceProvider<TEntity> where TEntity : IAARPEntity 
    {
        TEntity[] GetList();
        TEntity Get(int key);
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(int key);
    }
}
