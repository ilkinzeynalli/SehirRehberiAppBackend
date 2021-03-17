using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts;
using SehirRehberi.DataAccess.Concrete.EntityFramework.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.DataAccess.Concrete.EntityFramework.Seeders
{
    public static class SeederIdentityData
    {
        public static async Task EnsurePopulated(IApplicationBuilder app,IConfiguration configuration)
        {
            var userManager = app.ApplicationServices.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = app.ApplicationServices.GetRequiredService<RoleManager<ApplicationRole>>();

            var context = app.ApplicationServices.GetRequiredService<ApplicationIdentityDbContext>();

            if (context is ApplicationIdentityDbContext)
            {
                context.Database.Migrate();

                if (!await userManager.Users.AnyAsync())
                {
                    var userName = configuration["Users:Admin:username"].ToString();
                    var email = configuration["Users:Admin:email"].ToString();
                    var password = configuration["Users:Admin:password"].ToString();
                    var role = configuration["Users:Admin:role"].ToString();

                    if (await userManager.FindByNameAsync(userName) == null)
                    {
                        if (await roleManager.FindByNameAsync(role) == null)
                        {
                            await roleManager.CreateAsync(new ApplicationRole()
                            {
                                Name = role
                            });
                        }

                        var applicationUser = new ApplicationUser()
                        {
                            UserName = userName,
                            Email = email
                        };

                        var result =  await userManager.CreateAsync(applicationUser,password);

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(applicationUser, role);
                        }

                    }
                }
            }
        }
    }
}
