using ApiLayer.Validators;
using DVLD.Application.Features.People.Common.Validators;
using FluentValidation;

namespace DVLD.Application.Features.People.Common.Requests.NationalNumber
{
    public class NationalNumberRequestValidator : AbstractValidator<NationalNumberRequest>
    {
        public NationalNumberRequestValidator()
        {
            RuleFor(req => req.NationalNumber).Requeired().ValidNationalNo();
        }
    }
}
