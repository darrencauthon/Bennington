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
       }
    }
}