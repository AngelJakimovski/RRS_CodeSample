using AARP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

namespace AARP.Services
{
    public abstract class BaseModelProvider<TEntity> : IResourceProvider<TEntity> where TEntity: class, IAARPEntity
    {
        public BaseModelProvider() { }

        public TEntity[] GetList()
        {
            using (var dbContext = new AARPDbContext())
            {
                return dbContext.Set<TEntity>().ToArray();
            }
        }

        public TEntity[] GetList(Expression<Func<TEntity, bool>> predicate)
        {
            using (var dbContext = new AARPDbContext())
            {
                return dbContext.Set<TEntity>().Where(predicate).ToArray();
            }
        }

        public TEntity Get(int key)
        {
            using (var dbContext = new AARPDbContext())
            {
                return dbContext.Set<TEntity>().Find(key);
            }
        }

        public TEntity Add(TEntity entity)
        {
            using (var dbContext = new AARPDbContext())
            {
                dbContext.Set<TEntity>().Add(entity);
                dbContext.SaveChanges();
            }

            return entity;
        }

        public TEntity[] AddRange(TEntity[] entities)
        {
            using (var dbContext = new AARPDbContext())
            {
                try
                {
                    dbContext.Set<TEntity>().AddRange(entities);
                    dbContext.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }

            return entities;
        }

        public TEntity Update(TEntity entity)
        {
            using (var dbContext = new AARPDbContext())
            {
                dbContext.Set<TEntity>().Attach(entity);
                dbContext.Entry(entity).State = EntityState.Modified;
                dbContext.SaveChanges();
            }

            return entity;
        }

        public void Delete(int key)
        {
            using (var dbContext = new AARPDbContext())
            {
                var entity = dbContext.Set<TEntity>().Find(key);
                if (entity != null)
                {
                    dbContext.Set<TEntity>().Remove(entity);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
