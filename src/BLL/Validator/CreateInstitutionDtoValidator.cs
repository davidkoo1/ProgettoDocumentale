using BLL.DTO.InstitutionDTOs;
using BLL.UserDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validator
{
    public class CreateInstitutionDtoValidator : AbstractValidator<CreateInstitutionDto>
    {
        public CreateInstitutionDtoValidator()
        {
            RuleFor(x => x.InstCode)
                .NotNull()
                .NotEmpty()
                .Length(5) 
                .Must(a => a != null && a.StartsWith("AC") == true);

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.AdditionalInfo)
                .MaximumLength(1000);
        }
    }
}
