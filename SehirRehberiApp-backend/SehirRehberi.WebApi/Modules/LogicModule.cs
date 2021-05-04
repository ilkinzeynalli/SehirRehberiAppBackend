using Microsoft.Extensions.DependencyInjection;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Concrete;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Repositories;
using SehirRehberi.WebApi.Services.Abstract;
using SehirRehberi.WebApi.Services.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.API.Modules
{
    internal class LogicModule
    {
        public static void Load(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, EfUnitOfWork>();

            services.AddTransient<IValueService, ValueManager>();
            services.AddTransient<ICityService, CityManager>();
            services.AddTransient<IPhotoService, PhotoManager>();
            services.AddTransient<ITokenService, TokenManager>();
            services.AddTransient<IAspNetUserTokenService, AspNetUserTokenManager>();

        }
    }
}
