using ClothingStore.Data.Requests;
using FluentValidation;

namespace ClothingStore.Data.Validators;

public class AssignRoleToUserRequestValidator : AbstractValidator<AssignRoleToUserRequest>
{
    public AssignRoleToUserRequestValidator()
    {
        RuleFor(x => x.Role)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0);
    }
}