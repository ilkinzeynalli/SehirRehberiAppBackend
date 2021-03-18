﻿using Microsoft.EntityFrameworkCore;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SehirRehberi.DataAccess.Concrete.EntityFramework.Repositories
{
    public class EfCityRepository : EfGenericRepositoryBase<City>,ICityRepository
    {
        public EfCityRepository(ApplicationIdentityDbContext context) : base(context)
        {
        }

        public ApplicationIdentityDbContext Context => _context as ApplicationIdentityDbContext;
        public override IQueryable<City> GetAll(Expression<Func<City, bool>> filter = null)
        {
            return Context.Cities.Include(i=>i.Photos);
        }
    }
}