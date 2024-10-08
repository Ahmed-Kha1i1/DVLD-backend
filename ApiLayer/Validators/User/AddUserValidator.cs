using ApiLayer.Services;
using ApiLayer.Validators.Person;
using DataLayerCore.User;
using FluentValidation;

namespace ApiLayer.Validators.User
{
    public class AddUserValidator : AbstractValidator<UserForCreateDTO>
    {
        public AddUserValidator(UserService userService)
        {
            Include(new ModificationUserValidator());
            RuleFor(user => user.UserName).ValidateUniqueUsername(userService);
            RuleFor(user => user.Password)
               .Requeired("Password")
               .MaximumLength(20)
               .MatchesPasswordPattern();
        }
    }
}
