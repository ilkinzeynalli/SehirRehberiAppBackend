using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Configurations;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts
{
    public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
                                            ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
                                            ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> context) : base(context)
        {

        }

        public virtual DbSet<Value> Values { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<ApplicationUserToken> ApplicationUserToken { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //If sql is not configured in Startup, we do it here.
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new ApplicationRoleConfiguration());
            builder.ApplyConfiguration(new ApplicationUserTokenConfiguration());
            builder.ApplyConfiguration(new CitiesConfiguration());
            builder.ApplyConfiguration(new PhotosConfiguration());
        }
    }
}
