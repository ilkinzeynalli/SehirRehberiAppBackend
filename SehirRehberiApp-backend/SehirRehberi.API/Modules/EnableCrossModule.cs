using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.WebApi.Modules
{
    internal class EnableCrossModule
    {
        public static void Load(IServiceCollection services, string name)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name, builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }
    }
}
