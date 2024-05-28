using BLL.DTO;
using BLL.UserDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validator
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Surname)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Patronymic)
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.RolesId)
                .NotNull().WithName("Role")
                .NotEmpty().WithName("Role");


            When(x => x.RolesId != null && x.RolesId.Contains(3), () =>
            {
                RuleFor(x => x.IdInstitution)
                    .NotNull().WithName("Institution")
                    .NotEmpty().WithName("Institution")
                    .WithMessage("For this role institution is requared");
            });
        }
    }
}
