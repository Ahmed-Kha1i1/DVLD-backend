using FluentValidation;

namespace ApiLayer.Validators
{
    public static class CustomValidators
    {

        public static IRuleBuilderOptions<T, string?> MatchesNamePattern<T>(this IRuleBuilder<T, string?> ruleBuilder, string fieldName)
        {
            return ruleBuilder.Matches(ValidationRegex.NamePattern)
                .WithMessage($"{fieldName} can only contain letters, numbers, and hyphens");
        }

        public static IRuleBuilderOptions<T, string?> MatchesPasswordPattern<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder.Matches(ValidationRegex.PasswordPattern)
                .WithMessage($"Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character");
        }

        public static IRuleBuilderOptions<T, TProperty> Requeired<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, string fieldName)
        {
            return ruleBuilder.NotEmpty().WithMessage($"{fieldName} is required");
        }

        public static int? GetIdFromRoute(IHttpContextAccessor accessor, string parameterName)
        {
            var PersonIdString = accessor.HttpContext?.Request.RouteValues[parameterName]?.ToString();

            if (int.TryParse(PersonIdString, out int Id))
            {
                return Id;
            }

            return null;
        }

    }
}
