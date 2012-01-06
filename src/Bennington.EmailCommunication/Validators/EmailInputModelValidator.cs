using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bennington.EmailCommunication.Models;
using Bennington.EmailCommunication.Validators.Helpers;
using FluentValidation;

namespace Bennington.EmailCommunication.Validators
{
    public class EmailInputModelValidator : AbstractValidator<EmailInputModel>
    {
        private IValidateSemiColonDelimitedEmailListHelper validateSemiColonDelimitedEmailListHelper;

        public EmailInputModelValidator(IValidateSemiColonDelimitedEmailListHelper validateSemiColonDelimitedEmailListHelper)
        {
            this.validateSemiColonDelimitedEmailListHelper = validateSemiColonDelimitedEmailListHelper;

            RuleFor(a => a.EngineerId).Must(b => !string.IsNullOrWhiteSpace(b)).WithMessage("Field required");
            RuleFor(a => a.FromEmail).EmailAddress().WithMessage("Invalid email address");
            RuleFor(a => a.FromEmail).Must(b => !string.IsNullOrWhiteSpace(b)).WithMessage("Field required");
            RuleFor(a => a.ToEmail).EmailAddress().When(b => !string.IsNullOrWhiteSpace(b.ToEmail)).WithMessage("Invalid email address");
            RuleFor(a => a.CcEmails).Must(b => validateSemiColonDelimitedEmailListHelper.IsThisAValidEmailList(b)).When(c => !string.IsNullOrWhiteSpace(c.CcEmails)).WithMessage("Invalid email address");
            RuleFor(a => a.BccEmails).Must(b => validateSemiColonDelimitedEmailListHelper.IsThisAValidEmailList(b)).When(c => !string.IsNullOrWhiteSpace(c.BccEmails)).WithMessage("Invalid email address");
        }
    }
}