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

        public static IRuleBuilderOptions<T, TProperty> Requeired<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, string fieldName)
        {
            return ruleBuilder.NotEmpty().WithMessage($"{fieldName} is required");
        }


    }
}
