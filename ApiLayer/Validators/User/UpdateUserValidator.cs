using ApiLayer.Services;
using ApiLayer.Validators.Person;
using DataLayerCore.User;
using FluentValidation;

namespace ApiLayer.Validators.User
{
    public class UpdateUserValidator : AbstractValidator<UserForUpdateDTO>
    {
        public UpdateUserValidator(IHttpContextAccessor accessor, UserService userService)
        {
            Include(new ModificationUserValidator());
            RuleFor(user => user.UserName).ValidateUniqueUsername(userService, accessor);
        }
    }
}
