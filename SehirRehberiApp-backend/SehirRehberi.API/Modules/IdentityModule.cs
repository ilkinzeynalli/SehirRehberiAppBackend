using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.API.Modules
{
    public static class IdentityModule
    {
        public static void Load(IServiceCollection services)
        {
            //Adding Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
                {
                    //Identity settings
                    opt.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

        }
    }
}
