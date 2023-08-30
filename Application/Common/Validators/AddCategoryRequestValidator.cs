using Application.Categories.Commands;
using FluentValidation;

namespace Application.Common.Validators;

public class AddCategoryRequestValidator : AbstractValidator<AddCategoryCommand>
{
    public AddCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
    }
}