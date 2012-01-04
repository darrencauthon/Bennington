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
    }
}