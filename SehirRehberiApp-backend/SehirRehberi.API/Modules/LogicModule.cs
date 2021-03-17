using Microsoft.Extensions.DependencyInjection;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Concrete;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.API.Modules
{
    public static class LogicModule
    {
        public static void Load(IServiceCollection services)
        {
            services.AddSingleton<IUnitOfWork, EfUnitOfWork>();

            services.AddScoped<IValueRepository, EfValueRepository>();
            services.AddScoped<IValueService, IValueManager>();
        }
    }
}
