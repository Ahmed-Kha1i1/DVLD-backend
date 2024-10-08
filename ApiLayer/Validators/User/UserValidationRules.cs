using ApiLayer.Services;
using FluentValidation;

namespace ApiLayer.Validators.User
{
    public static class UserValidationRules
    {
        private static string _UserId = "userId";
        public static IRuleBuilderOptions<T, string> ValidateUniqueUsername<T>(this IRuleBuilder<T, string> ruleBuilder, UserService userService, IHttpContextAccessor? accessor = null)
        {
            return ruleBuilder.MustAsync(async (username, cancellation) =>
            {
                int? Id = accessor is not null ? CustomValidators.GetIdFromRoute(accessor, _UserId) : null;
                return await userService.BeUniqueUsername(username, Id);
            }).WithMessage("Email must be unique");

        }
    }
}
