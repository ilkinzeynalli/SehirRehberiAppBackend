using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SehirRehberi.WebApi.Dtos;
using SehirRehberi.WebApi.FluentValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.WebApi.Modules
{
    internal class ValidatorModule
    {
        public static void Load(IServiceCollection services)
        {
            services.AddTransient<IValidator<UserForLoginDTO>, UserForLoginDTOValidator>();
            services.AddTransient<IValidator<UserForRegisterDTO>, UserForRegisterDTOValidator>();
            services.AddTransient<IValidator<UserForChangePasswordDTO>, UserForChangePasswordDTOValidator>();
            services.AddTransient<IValidator<TokensForRefreshDTO>, TokensForRefreshDTOValidator>();
        }
    }
}
