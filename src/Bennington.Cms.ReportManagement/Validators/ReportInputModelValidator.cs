using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bennington.Cms.ReportManagement.Models;
using FluentValidation;

namespace Bennington.Cms.ReportManagement.Validators
{
    public class ReportInputModelValidator : AbstractValidator<ReportInputModel>
    {
        public ReportInputModelValidator()
        {
            RuleFor(a => a.Name).Must(b => !string.IsNullOrWhiteSpace(b)).WithMessage("Field Required");
        }

        public override FluentValidation.Results.ValidationResult Validate(ReportInputModel instance)
        {
            return base.Validate(instance);
        }
    }
}
