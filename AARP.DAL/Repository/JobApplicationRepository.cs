using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AARP.DAL
{
    public class JobApplicationRepository : IJobApplicationRepository
    {

        public IList<JobApplication> List()
        {
            using (var db = new ScopicAARPEntities())
            {
                return db.JobApplications.ToList();
            }
        }

        public JobApplication FindById(string id)
        {
            using (var db = new ScopicAARPEntities())
            {
                return db.JobApplications.Find(int.Parse(id));
            }
        }

        public void Add(JobApplication entity)
        {
            using (var db = new ScopicAARPEntities())
            {
                db.JobApplications.Add(entity);
                db.SaveChanges();
            }
        }

        public void Update(JobApplication entity)
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
                var entity = db.JobApplications.Find(int.Parse(id));
                if (entity == null)
                    return;
                db.JobApplications.Remove(entity);
                db.SaveChanges();
            }
        }
    }
}
