using FluentValidation;
using FluentValidation.Results;

namespace Application.Common.Validators;

public static class FluentModelValidator
{
    public static ValidationResult Execute<TValidator, TModel>(TModel model) where TValidator : AbstractValidator<TModel>, new()
    {
        var validator = new TValidator();
        return validator.Validate(model);
    }

    public static async Task<ValidationResult> ExecuteAsync<TValidator, TModel>(TModel model) where TValidator : AbstractValidator<TModel>, new()
    {
        var validator = new TValidator();
        var result = await validator.ValidateAsync(model);
        return result;
    }
}