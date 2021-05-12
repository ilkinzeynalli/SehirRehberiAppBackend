using FluentValidation;
using SehirRehberi.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.Business.ValidationRules.FluentValidation
{
    public class TokensDtoValidator : AbstractValidator<TokensDto>
    {
        public TokensDtoValidator()
        {
            RuleFor(r => r.AccessToken).NotEmpty().WithMessage("Tokeni daxil edin");
            RuleFor(r => r.AccessToken).NotNull().WithMessage("Token null ola bilmez");

            RuleFor(r => r.RefreshToken).NotEmpty().WithMessage("Tokeni daxil edin");
            RuleFor(r => r.RefreshToken).NotNull().WithMessage("Token null ola bilmez");

        }
    }
}
