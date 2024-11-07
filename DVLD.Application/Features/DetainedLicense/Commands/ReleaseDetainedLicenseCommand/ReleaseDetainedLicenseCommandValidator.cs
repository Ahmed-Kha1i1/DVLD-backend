namespace DVLD.Application.Features.DetainedLicense.Commands.ReleaseDetainedLicenseCommand
{
    public class ReleaseDetainedLicenseCommandValidator : AbstractValidator<ReleaseDetainedLicenseCommand>
    {
        public ReleaseDetainedLicenseCommandValidator(IDetainedLicenseRepository detainedLicenseRepository)
        {
            RuleFor(x => x.ReleasedByUserID).ValidId();
            RuleFor(x => x.LicenseId).ValidId()
                .MustAsync(async (licnseId, cancellation) => await detainedLicenseRepository.IsLicenseDetained(licnseId)).WithMessage("License is not detained");


        }
    }
}
