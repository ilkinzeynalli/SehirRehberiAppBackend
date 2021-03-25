using FluentValidation;
using SehirRehberi.WebApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.WebApi.FluentValidations
{
    public class UserForLoginDTOValidator : AbstractValidator<UserForLoginDTO>
    {
        public UserForLoginDTOValidator()
        {
            RuleFor(r => r.UserName).NotEmpty().WithMessage("Username daxil edin");
            RuleFor(r => r.UserName).NotNull().WithMessage("Username null ola bilmez");

            RuleFor(r => r.Password).NotEmpty().WithMessage("Passwordu daxil edin");
            RuleFor(r => r.Password).NotNull().WithMessage("Password null ola bilmez");

        }
    }
}
