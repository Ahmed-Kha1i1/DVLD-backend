using DVLD.Application.Features.People.Common.Validators;

namespace DVLD.Application.Features.People.Common.Requests.Phone.Unique
{
    public class PhoneUniqueRequestValidator : AbstractValidator<PhoneUniqueRequest>
    {
        public PhoneUniqueRequestValidator()
        {
            RuleFor(req => req.Phone).Requeired().ValidPhone();
            RuleFor(req => req.Id).Must(id => id == null || id > 0).WithMessage("{PropertyName} must be greater than 0");
        }
    }
}
