using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.DAL
{
    public class JobPositionTypeRepository : IJobPositionTypeRepository
    {
        public IList<JobPositionType> List()
        {
            using (var db = new ScopicAARPEntities())
            {
                return db.JobPositionTypes.ToList();
            }
        }

        public JobPositionType FindById(string id)
        {
            using (var db = new ScopicAARPEntities())
            {
                return db.JobPositionTypes.Find(int.Parse(id));
            }
        }

        public void Add(JobPositionType entity)
        {
            using (var db = new ScopicAARPEntities())
            {
                db.JobPositionTypes.Add(entity);
                db.SaveChanges();
            }
        }

        public void Update(JobPositionType entity)
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
                var entity = db.JobPositionTypes.Find(int.Parse(id));
                if (entity == null)
                    return;
                db.JobPositionTypes.Remove(entity);
                db.SaveChanges();
            }
        }
    }
}
