using FluentValidation;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.Business.ValidationRules.FluentValidation
{
    public class CityValidator : AbstractValidator<City>
    {
        public CityValidator()
        {
            RuleFor(c => c.CityName).NotEmpty();
            RuleFor(c => c.CityName).MinimumLength(4);
            RuleFor(c => c.UserId).NotEmpty();
        }
    }
}
