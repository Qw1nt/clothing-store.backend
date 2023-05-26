using ClothingStore.Data.Requests;
using FluentValidation;

namespace ClothingStore.Data.Validators;

public class CreateProductRequestValidator : AbstractValidator<AddProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.CategoriesIds.Count)
            .NotNull()
            .GreaterThan(0);
    }
}