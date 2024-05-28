using BLL.Common.Interfaces;
using BLL.Common.Repository;
using BLL.DTO.InstitutionDTOs;
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
    public class CreateInstitutionDtoValidator : AbstractValidator<CreateInstitutionDto>
    {
        private readonly IInstitutionRepository _institutionRepository;
        public CreateInstitutionDtoValidator(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;

            RuleFor(x => x.InstCode)
                .NotNull()
                .NotEmpty()
                .Length(5) 
                .Must(a => a != null && a.StartsWith("AC") == true).WithMessage("Code start with ACXXX")
                .MustAsync(BeUniqueCode).WithMessage("This Code is already in use");

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.AdditionalInfo)
                .MaximumLength(1000);
        }
        private async Task<bool> BeUniqueCode(string code, CancellationToken cancellationToken)
        {
            return !await _institutionRepository.IsCodeInUseAsync(code);
        }
    }
}
