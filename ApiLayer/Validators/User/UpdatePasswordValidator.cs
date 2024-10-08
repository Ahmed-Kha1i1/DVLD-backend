using DataLayerCore.User;
using FluentValidation;

namespace ApiLayer.Validators.User
{
    public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordDTO>
    {
        public UpdatePasswordValidator()
        {
            RuleFor(user => user.Password)
               .Requeired("Password")
               .MaximumLength(20)
               .MatchesPasswordPattern();
        }
    }
}
