using FluentValidation;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.Business.ValidationRules.FluentValidation
{
    public class ValueValidator : AbstractValidator<Value>
    {
        public ValueValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Name).Must(StartWithV).WithMessage("Giriline valude degeri 'V' ile baslamali");
        }

        private bool StartWithV(string arg)
        {
            return arg.StartsWith("V");
        }
    }
}
