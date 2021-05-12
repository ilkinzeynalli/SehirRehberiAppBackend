using Microsoft.EntityFrameworkCore;
using SehirRehberi.Core.DataAccess.EntityFramework;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.DataAccess.Concrete.EntityFramework.Repositories
{
    public class EfCityRepository : EfGenericRepositoryBase<City>,ICityDal
    {
        public EfCityRepository(ApplicationIdentityDbContext context) : base(context)
        {
        }

        public ApplicationIdentityDbContext Context => _context as ApplicationIdentityDbContext;

    }
}
