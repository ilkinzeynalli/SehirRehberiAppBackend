using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.DataAccess.Concrete.EntityFramework.Repositories
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private ApplicationIdentityDbContext _context;

        public EfUnitOfWork(ApplicationIdentityDbContext context)
        {
            _context = context;
        }


        private IValueRepository _values;
        private ICityRepository _cities;
        private IPhotoRepository _photos;
        private IAspNetUserToken _aspNetUserTokens;

        public IValueRepository Values => (_values ?? (_values = new EfValueRepository(_context)));

        public ICityRepository Cities => (_cities ?? (_cities = new EfCityRepository(_context)));

        public IPhotoRepository Photos => (_photos ?? (_photos = new EfPhotoRepository(_context)));
        public IAspNetUserToken AspNetUserTokens => (_aspNetUserTokens ?? (_aspNetUserTokens = new EfAspNetUserToken(_context)));

        public async Task<int> Complete()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception("Database save ederken problem yarandi");
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
