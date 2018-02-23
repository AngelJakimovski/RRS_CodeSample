using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.DAL
{
    public class AppConfigurationRepository : IConfigurationRepository
    {
        public IList<Configuration> List()
        {
            using (var db = new ScopicAARPEntities())
            {
                return db.Configurations.ToList();
            }
        }

        public Configuration FindById(string id)
        {
            using (var db = new ScopicAARPEntities())
            {
                return db.Configurations.Find(int.Parse(id));
            }
        }

        public void Add(Configuration entity)
        {
            using (var db = new ScopicAARPEntities())
            {
                db.Configurations.Add(entity);
                db.SaveChanges();
            }
        }

        public void Update(Configuration entity)
        {
            using (var db = new ScopicAARPEntities())
            {
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            using (var db = new ScopicAARPEntities())
            {
                var item = FindById(id);
                if (item == null)
                    return;
                db.Configurations.Remove(item);
                db.SaveChanges();
            }
        }
    }
}
