using BLL.DTO.DocumentDTOs;
using BLL.DTO.InstitutionDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
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
                .NotEmpty()
                .NotNull().WithMessage("File is required.")
                .Must(file => file.ContentLength > 0).WithMessage("File cannot be empty.")
                .Must(file => file.ContentLength < 10 * 1024 * 1024).WithMessage("File size must be less than 10MB.");
                //.Must(file =>
                //{
                //    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                //    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                //    return allowedExtensions.Contains(extension);
                //}).WithMessage("Invalid file type. Only .jpg, .jpeg, .png, .gif are allowed.")

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
