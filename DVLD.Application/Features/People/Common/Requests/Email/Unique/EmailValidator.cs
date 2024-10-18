using ApiLayer.Validators;
using DVLD.Application.Features.People.Common.Validators;
using FluentValidation;

namespace DVLD.Application.Features.People.Common.Requests.Email.Unique
{
    public class EmailValidator : AbstractValidator<EmailUniqueRequest>
    {
        public EmailValidator()
        {
            RuleFor(req => req.Email).Requeired().ValidEmail();
            RuleFor(req => req.Id).ValidId();
        }
    }
}
