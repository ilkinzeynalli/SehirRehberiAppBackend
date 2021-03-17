using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.DataAccess.Concrete.EntityFramework.Repositories
{
    public class EfValueRepository : EfGenericRepositoryBase<Value>,IValueRepository
    {
        public EfValueRepository(ApplicationIdentityDbContext context)
            :base(context)
        {
        }

        public ApplicationIdentityDbContext Context
        {
            get
            {
                return _context as ApplicationIdentityDbContext;
            }
        }
    }
}
