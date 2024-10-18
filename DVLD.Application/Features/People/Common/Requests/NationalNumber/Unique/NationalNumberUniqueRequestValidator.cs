using ApiLayer.Validators;
using DVLD.Application.Features.People.Common.Validators;
using FluentValidation;

namespace DVLD.Application.Features.People.Common.Requests.NationalNumber
{
    public class NationalNumberUniqueRequestValidator : AbstractValidator<NationalNumberUniqueRequest>
    {
        public NationalNumberUniqueRequestValidator()
        {
            RuleFor(req => req.NationalNumber).Requeired().ValidNationalNo();
            RuleFor(req => req.Id).ValidId();
        }
    }
}
