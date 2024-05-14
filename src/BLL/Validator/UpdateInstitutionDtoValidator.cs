using BLL.DTO.InstitutionDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validator
{
    public class UpdateInstitutionDtoValidator : AbstractValidator<UpdateInstitutionDto>
    {
        public UpdateInstitutionDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.AdditionalInfo)
                .MaximumLength(1000);
        }
    }
}
