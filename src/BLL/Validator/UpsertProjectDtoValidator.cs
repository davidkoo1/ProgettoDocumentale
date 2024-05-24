using BLL.DTO.ProjectDTOs;
using BLL.UserDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validator
{
    public class UpsertProjectDtoValidator : AbstractValidator<UpsertProjectDto>
    {
        public UpsertProjectDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(255);

            RuleFor(x => x.InstitutionId).NotEmpty().NotNull();

            RuleFor(x => x.DateFrom)
                 .LessThanOrEqualTo(x => x.DateTill)
                 .NotEmpty();

            RuleFor(x => x.AdditionalInfo).MaximumLength(999);
        }
    }
}
