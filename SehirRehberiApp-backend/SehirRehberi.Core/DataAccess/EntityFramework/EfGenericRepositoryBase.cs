using Microsoft.EntityFrameworkCore;
using SehirRehberi.Core.DataAccess;
using SehirRehberi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Core.DataAccess.EntityFramework
{
    public class EfGenericRepositoryBase<T> : IGenericRepository<T> 
        where T : class, IEntity, new()
    {
        protected readonly DbContext _context;
        
        public EfGenericRepositoryBase(DbContext context)
        {
            _context = context;
        }


        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ?  _context.Set<T>() :  _context.Set<T>().Where(filter);
        }
        public virtual async Task<T> GetEntityById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public virtual T Add(T entity)
        {
            _context.Add<T>(entity);
            return entity;
        }
        public virtual T Update(T entity)
        {
            _context.Update<T>(entity);
            return entity;
        }
        public virtual void Delete(T entity)
        {
            _context.Remove<T>(entity);
        }
        public virtual async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

       
    }
}
