using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.DAL
{
    public class ReviewerRepository : IReviewerRepository
    {
        public IList<Reviewer> List()
        {
            using (var db = new ScopicAARPEntities())
            {
                return db.Reviewers.ToList();
            }
        }

        public Reviewer FindById(string id)
        {
            using (var db = new ScopicAARPEntities())
            {
                return db.Reviewers.Find(int.Parse(id));
            }
        }

        public void Add(Reviewer entity)
        {
            using (var db = new ScopicAARPEntities())
            {
                db.Reviewers.Add(entity);
                db.SaveChanges();
            }
        }

        public void Update(Reviewer entity)
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
                var entity = db.Reviewers.Find(int.Parse(id));
                if (entity == null)
                    return;
                db.Reviewers.Remove(entity);
                db.SaveChanges();
            }
        }
    }
}
