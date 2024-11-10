using DVLD.Application.Features.People.Common.Validators;

namespace DVLD.Application.Features.People.Common.Requests.Email.Unique
{
    public class EmailValidator : AbstractValidator<EmailUniqueRequest>
    {
        public EmailValidator()
        {
            RuleFor(req => req.Email).Requeired().ValidEmail();
            RuleFor(req => req.Id).Must(id => id == null || id > 0).WithMessage("{PropertyName} must be greater than 0");
        }
    }
}
