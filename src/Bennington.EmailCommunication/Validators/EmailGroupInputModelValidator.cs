using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bennington.EmailCommunication.Models;
using FluentValidation;

namespace Bennington.EmailCommunication.Validators
{
    public class EmailGroupInputModelValidator : AbstractValidator<EmailGroupInputModel>
    {
        public EmailGroupInputModelValidator()
        {
            RuleFor(a => a.Name).Must(a => !string.IsNullOrWhiteSpace(a)).WithMessage("Field required");
            RuleFor(a => a.EmailCount).Must(b => b > 0).WithMessage("You must have at least one email");
            RuleFor(a => a.EngineerId).Must(b => !string.IsNullOrWhiteSpace(b)).WithMessage("Specify an id");
        }
    }
}