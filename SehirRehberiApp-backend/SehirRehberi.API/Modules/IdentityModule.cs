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
    internal class IdentityModule
    {
        public static void Load(IServiceCollection services)
        {
            //Adding Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
                {
                    //Identity settings
                    opt.User.RequireUniqueEmail = false;
                })
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

        }
    }
}
