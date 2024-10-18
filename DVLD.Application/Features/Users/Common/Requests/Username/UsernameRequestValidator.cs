using ApiLayer.Validators;
using DVLD.Application.Features.Users.Common.Validators;
using FluentValidation;

namespace DVLD.Application.Features.Users.Common.Requests.Username
{
    public class UsernameRequestValidator : AbstractValidator<UsernameRequest>
    {
        public UsernameRequestValidator()
        {
            RuleFor(req => req.Username).Requeired().ValidateUsername();
        }
    }
}
