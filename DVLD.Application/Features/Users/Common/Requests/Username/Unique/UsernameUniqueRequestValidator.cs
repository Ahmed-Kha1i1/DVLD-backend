using ApiLayer.Validators;
using DVLD.Application.Features.Users.Common.Validators;
using FluentValidation;

namespace DVLD.Application.Features.Users.Common.Requests.Username.Unique
{
    public class UsernameUniqueRequestValidator : AbstractValidator<UsernameUniqueRequest>
    {
        public UsernameUniqueRequestValidator()
        {
            RuleFor(req => req.Username).Requeired().ValidateUsername();
            RuleFor(req => req.Id).ValidId();
        }
    }
}
