using BLL.DTO;
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
                .NotNull()
                .NotEmpty();
        }
    }
}
