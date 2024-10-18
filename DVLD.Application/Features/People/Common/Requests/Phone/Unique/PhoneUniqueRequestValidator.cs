using ApiLayer.Validators;
using DVLD.Application.Features.People.Common.Validators;
using FluentValidation;

namespace DVLD.Application.Features.People.Common.Requests.Phone.Unique
{
    public class PhoneUniqueRequestValidator : AbstractValidator<PhoneUniqueRequest>
    {
        public PhoneUniqueRequestValidator()
        {
            RuleFor(req => req.Phone).Requeired().ValidPhone();
            RuleFor(req => req.Id).ValidId();
        }
    }
}
