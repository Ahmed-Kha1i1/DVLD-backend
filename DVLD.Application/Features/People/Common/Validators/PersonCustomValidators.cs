using ApiLayer.Validators;
using FluentValidation;

namespace DVLD.Application.Features.People.Common.Validators
{
    public static class PersonCustomValidators
    {
        public static IRuleBuilderOptions<T, string?> ValidNationalNo<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder.MaximumLength(20).MatchesNamePattern();
        }

        public static IRuleBuilderOptions<T, string?> ValidEmail<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder.EmailAddress().WithMessage("Invalid email format");
        }

        public static IRuleBuilderOptions<T, string> ValidPhone<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(ValidationRegex.PhonePattern).WithMessage("Phone is not valid");
        }
    }
}
