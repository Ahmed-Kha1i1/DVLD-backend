using ApiLayer.Validators;
using FluentValidation;

namespace DVLD.Application.Features.License.Common.Requests.IsLicenseExistsRequest
{
    public class IsLicenseExistsRequestValidator : AbstractValidator<IsLicenseExistsRequest>
    {
        public IsLicenseExistsRequestValidator()
        {
            RuleFor(req => req.PersonId).ValidId();
            RuleFor(req => req.LicenseClassId).ValidId();
        }
    }
}
