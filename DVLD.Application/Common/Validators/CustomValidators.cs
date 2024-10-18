using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ApiLayer.Validators
{
    public static class CustomValidators
    {

        public static IRuleBuilderOptions<T, string?> MatchesNamePattern<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder.Matches(ValidationRegex.NamePattern)
                .WithMessage("{PropertyName} can only contain letters, numbers, and hyphens");
        }

        public static IRuleBuilderOptions<T, string?> MatchesPasswordPattern<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder.Matches(ValidationRegex.PasswordPattern)
                .WithMessage("Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character");
        }

        public static IRuleBuilderOptions<T, TProperty> Requeired<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.NotEmpty().WithMessage("{PropertyName} is required");
        }


        public static IRuleBuilderOptions<T, int> ValidId<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder.GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

        }

        public static IRuleBuilderOptions<T, int?> ValidId<T>(this IRuleBuilder<T, int?> ruleBuilder)
        {
            return ruleBuilder
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0").When(x => x != null);

        }
        public static int? GetIdFromRoute(IHttpContextAccessor? accessor, string parameterName)
        {
            if (accessor is null)
                return null;

            var PersonIdString = accessor.HttpContext?.Request.RouteValues[parameterName]?.ToString();

            if (int.TryParse(PersonIdString, out int Id))
            {
                return Id;
            }

            return null;
        }
    }
}
