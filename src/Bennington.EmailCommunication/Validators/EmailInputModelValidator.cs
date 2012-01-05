using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bennington.EmailCommunication.Models;
using FluentValidation;

namespace Bennington.EmailCommunication.Validators
{
    public class EmailInputModelValidator : AbstractValidator<EmailInputModel>
    {
        public EmailInputModelValidator()
        {
            RuleFor(a => a.EngineerId).Must(b => !string.IsNullOrWhiteSpace(b)).WithMessage("Field required");
            RuleFor(a => a.FromEmail).EmailAddress().WithMessage("Invalid email address");
            RuleFor(a => a.FromEmail).Must(b => !string.IsNullOrWhiteSpace(b)).WithMessage("Field required");
            RuleFor(a => a.ToEmail).EmailAddress().When(b => !string.IsNullOrWhiteSpace(b.ToEmail)).WithMessage("Invalid email address");
        }
    }
}