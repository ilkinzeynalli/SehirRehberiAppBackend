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
        private readonly ApplicationIdentityDbContext _context;

        public EfUnitOfWork(ApplicationIdentityDbContext context)
        {
            _context = context;
        }


        private IValueDal _values;
        private ICityDal _cities;
        private IPhotoDal _photos;
        private IAspNetUserTokenDal _aspNetUserTokens;

        public IValueDal Values => (_values ?? (_values = new EfValueRepository(_context)));

        public ICityDal Cities => (_cities ?? (_cities = new EfCityRepository(_context)));

        public IPhotoDal Photos => (_photos ?? (_photos = new EfPhotoRepository(_context)));
        public IAspNetUserTokenDal AspNetUserTokens => (_aspNetUserTokens ?? (_aspNetUserTokens = new EfAspNetUserToken(_context)));

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
