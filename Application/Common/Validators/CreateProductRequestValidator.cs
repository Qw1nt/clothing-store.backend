using Application.Products.Commands;
using FluentValidation;

namespace Application.Common.Validators;

public class CreateProductRequestValidator : AbstractValidator<AddProductCommand>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.CategoriesIds.Count)
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}