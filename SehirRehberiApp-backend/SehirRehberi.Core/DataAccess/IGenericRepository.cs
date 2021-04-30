using SehirRehberi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Core.DataAccess
{
    public interface IGenericRepository<T> where T : class, IEntity, new()
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null);
        Task<T> GetEntityById(int id);
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
        Task<int> SaveChanges();

    }
}
