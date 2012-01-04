using Bennington.Core.List;
using FluentValidation;

namespace Bennington.Cms.Validators
{
    public class ListManageValidators : AbstractValidator<DefaultListColumnProvider.SortingValues>
    {
    }

    public class ListViewModelValidator : AbstractValidator<ListViewModel>
    {
    }
}