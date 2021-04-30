using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SehirRehberi.Entities.DTOs;
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
            services.AddTransient<IValidator<UserForLoginDto>, UserForLoginDtoValidator>();
            services.AddTransient<IValidator<UserForRegisterDto>, UserForRegisterDtoValidator>();
            services.AddTransient<IValidator<UserForChangePasswordDto>, UserForChangePasswordDtoValidator>();
            services.AddTransient<IValidator<TokensForRefreshDto>, TokensForRefreshDtoValidator>();
        }
    }
}
