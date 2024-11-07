namespace DVLD.Application.Features.InternationalLicense.Commands.AddInternationalLicenseCommand
{
    public class AddInternationalLicenseCommandValidator : AbstractValidator<AddInternationalLicenseCommand>
    {
        public AddInternationalLicenseCommandValidator(IInternationalLicenseRepository internationalLicenseRepository)
        {
            RuleFor(x => x.LicenseId).ValidId();
            RuleFor(x => x.CreatedUserId).ValidId();

        }
    }
}
