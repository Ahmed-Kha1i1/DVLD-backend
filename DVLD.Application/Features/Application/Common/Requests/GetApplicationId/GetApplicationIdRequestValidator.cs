using ApiLayer.Validators;
using FluentValidation;

namespace DVLD.Application.Features.Application.Common.Requests.GetApplicationId
{
    public class GetApplicationIdRequestValidator : AbstractValidator<GetApplicationIdRequest>
    {
        public GetApplicationIdRequestValidator()
        {
            RuleFor(req => req.PersonId).ValidId();
            RuleFor(req => req.LicenseClassId).ValidId();
        }
    }
}
