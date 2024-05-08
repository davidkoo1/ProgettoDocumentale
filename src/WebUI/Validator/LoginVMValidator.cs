using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Models;

namespace WebUI.Validator
{
    public class LoginVMValidator : AbstractValidator<LoginVM>
    {
        public LoginVMValidator()
        {
            RuleFor(x => x.Username)
            .NotNull()
            .WithMessage("Please enter a username");
        }
    }
}