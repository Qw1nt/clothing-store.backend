using ClothingStore.Data.Requests;
using FluentValidation;

namespace ClothingStore.Data.Validators;

public class CheckoutRequestValidator : AbstractValidator<CheckoutRequest>
{
    public CheckoutRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .NotNull();
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull();
        
        RuleFor(x => x.CartItems)
            .NotEmpty()
            .NotNull();
        
        RuleFor(x => x.Address)
            .NotEmpty()
            .NotNull();
    }
}