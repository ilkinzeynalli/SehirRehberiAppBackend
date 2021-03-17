using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SehirRehberi.DataAccess.Concrete.EntityFramework.IdentityModels;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts
{
    public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> context) : base(context)
        {

        }

        public virtual DbSet<Value> Values { get; set; }
    }
}
