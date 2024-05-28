using BLL.DTO.DocumentDTOs;
using BLL.DTO.InstitutionDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validator
{
    public class CreateDocumentDtoValidator : AbstractValidator<CreateDocumentDto>
    {
        public CreateDocumentDtoValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.GroupingDate)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.InstitutionId).NotEmpty().NotNull();

            RuleFor(x => x.MacroId).NotEmpty().NotNull();



            RuleFor(x => x.AdditionalInfo)
                .MaximumLength(1000);


            When(x => x.MacroId == 3, () =>
            {
                RuleFor(x => x.ProjectId)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Project is required when select this Macro");
            });


            When(x => x.MacroId != 2, () =>
            {
                RuleFor(x => x.MicroId)
                    .NotNull().NotEmpty();
            });

        }
    }
}
