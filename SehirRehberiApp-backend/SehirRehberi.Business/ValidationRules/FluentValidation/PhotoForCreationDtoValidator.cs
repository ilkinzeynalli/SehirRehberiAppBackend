using FluentValidation;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.Business.ValidationRules.FluentValidation
{
    public class PhotoForCreationDtoValidator : AbstractValidator<PhotoForCreationDto>
    {
        public PhotoForCreationDtoValidator()
        {
            RuleFor(p => p.File).NotEmpty();
            RuleFor(p => p.DateAdded).NotEmpty();
            RuleFor(p => p.Description).NotEmpty();
        }
    }
}
