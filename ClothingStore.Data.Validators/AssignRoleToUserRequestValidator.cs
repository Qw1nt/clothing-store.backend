﻿using ClothingStore.Configurations;
using ClothingStore.Data.Requests;
using FluentValidation;

namespace ClothingStore.Data.Validators;

public class AssignRoleToUserRequestValidator : AbstractValidator<AssignRoleToUserRequest>
{
    public AssignRoleToUserRequestValidator()
    {
        RuleFor(x => x.Role)
            .NotEmpty()
            .NotNull()
            .Must(x => x 
                is IdentityConfiguration.Roles.Admin
                or IdentityConfiguration.Roles.Manager
                or IdentityConfiguration.Roles.User);

        RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0);
    }
}