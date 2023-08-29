using Application.UserCart.Commands;
using FluentValidation;

namespace Application.Common.Validators;

public class CheckoutRequestValidator : AbstractValidator<CheckoutCommandData>
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