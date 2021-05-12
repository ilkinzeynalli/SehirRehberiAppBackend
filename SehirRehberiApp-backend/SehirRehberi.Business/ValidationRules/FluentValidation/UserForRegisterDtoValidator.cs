using FluentValidation;
using SehirRehberi.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.Business.ValidationRules.FluentValidation
{
    public class UserForRegisterDtoValidator : AbstractValidator<UserForRegisterDto>
    {
        public UserForRegisterDtoValidator()
        {
            RuleFor(r => r.UserName).NotEmpty().WithMessage("User name daxil edin");
            RuleFor(r => r.UserName).NotNull().WithMessage("User name null ola bilmez");

            RuleFor(r => r.Password).NotEmpty().WithMessage("Passwordu daxil edin");
            RuleFor(r => r.Password).NotNull().WithMessage("Passwordu null ola bilmez");

            RuleFor(r => r.ConfirmPassword).NotEmpty().WithMessage("Təsdiq passwordunu daxil edin");
            RuleFor(r => r.ConfirmPassword).NotNull().WithMessage("Təsdiq passwordu null ola bilmez");


            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.Password != x.ConfirmPassword)
                {
                    context.AddFailure(nameof(x.Password), "Passwordlar uygun gelmir");
                }
            });

        }
    }
}
