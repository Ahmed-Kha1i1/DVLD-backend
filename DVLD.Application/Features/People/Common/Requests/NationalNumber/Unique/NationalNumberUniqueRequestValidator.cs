using DVLD.Application.Features.People.Common.Validators;

namespace DVLD.Application.Features.People.Common.Requests.NationalNumber
{
    public class NationalNumberUniqueRequestValidator : AbstractValidator<NationalNumberUniqueRequest>
    {
        public NationalNumberUniqueRequestValidator()
        {
            RuleFor(req => req.NationalNumber).Requeired().ValidNationalNo();
            RuleFor(req => req.Id).Must(id => id == null || id > 0).WithMessage("{PropertyName} must be greater than 0");
        }
    }
}
