using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.DataAccess.Concrete.EntityFramework.Seeders
{
    public static class SeederData
    {
        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<ApplicationIdentityDbContext>();

            if (context is ApplicationIdentityDbContext)
            {
                context.Database.Migrate();

                if (!await context.Values.AnyAsync())
                {
                    List<Value> values = new List<Value>()
                    {
                        new Value(){Name="Value 1"},
                        new Value(){Name="Value 2"},
                        new Value(){Name="Value 3"},
                        new Value(){Name="Value 4"}
                    };

                    await context.Values.AddRangeAsync(values);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
