using ApiLayer.Validators;
using FluentValidation;

namespace DVLD.Application.Features.Users.Common.Validators
{
    public static class UserCustomValidators
    {
        private static string _UserId = "userId";
        public static IRuleBuilderOptions<T, string?> ValidateUsername<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(20)
                .MatchesNamePattern();
        }

        public static IRuleBuilderOptions<T, string?> ValidPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(20)
               .MatchesPasswordPattern();
        }

    }
}
