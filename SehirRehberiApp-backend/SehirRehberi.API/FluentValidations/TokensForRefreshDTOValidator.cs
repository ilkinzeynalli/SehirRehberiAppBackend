using FluentValidation;
using SehirRehberi.WebApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.WebApi.FluentValidations
{
    public class TokensForRefreshDTOValidator : AbstractValidator<TokensForRefreshDTO>
    {
        public TokensForRefreshDTOValidator()
        {
            RuleFor(r => r.AccessToken).NotEmpty().WithMessage("Tokeni daxil edin");
            RuleFor(r => r.AccessToken).NotNull().WithMessage("Token null ola bilmez");

            RuleFor(r => r.RefreshToken).NotEmpty().WithMessage("Tokeni daxil edin");
            RuleFor(r => r.RefreshToken).NotNull().WithMessage("Token null ola bilmez");

        }
    }
}
