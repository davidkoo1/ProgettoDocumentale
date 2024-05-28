using BLL.Common.Interfaces;
using BLL.DTO;
using BLL.UserDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Validator
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        private readonly IUserRepository _userRepository;
        public CreateUserDtoValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.UserName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(7)
                .MinimumLength(5)
                .Must(a => a != null && a.StartsWith("Cr") == true).WithMessage("Username start with CrXXX")
                .MustAsync(BeUniqueUsername).WithMessage("Username is already in use");

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
                .MaximumLength(100)
                .MustAsync(BeUniqueEmail).WithMessage("Email is already in use");

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
        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return !await _userRepository.IsEmailInUseAsync(email);
        }
        private async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken)
        {
            return !await _userRepository.IsUserNameInUseAsync(username);
        }
    }
}
