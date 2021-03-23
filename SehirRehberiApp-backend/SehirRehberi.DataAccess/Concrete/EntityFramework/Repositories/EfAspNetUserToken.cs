﻿using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.DataAccess.Concrete.EntityFramework.Repositories
{
    public class EfAspNetUserToken : EfGenericRepositoryBase<ApplicationUserToken>,IAspNetUserToken
    {
        public EfAspNetUserToken(ApplicationIdentityDbContext context)
            :base(context)
        {

        }

        public ApplicationIdentityDbContext Context => _context as ApplicationIdentityDbContext;
        
    }
}