using ClothingStore.Data.Requests;
using FluentValidation;
using FluentValidation.Results;

namespace ClothingStore.Data.Validators;

public class AddCategoryRequestValidator : AbstractValidator<AddCategoryRequest>
{
    public AddCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
    }
}