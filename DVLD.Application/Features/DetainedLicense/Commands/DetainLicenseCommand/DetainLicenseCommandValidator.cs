namespace DVLD.Application.Features.DetainedLicense.Commands.DetainLicenseCommand
{
    public class DetainLicenseCommandValidator : AbstractValidator<DetainLicenseCommand>
    {
        public DetainLicenseCommandValidator(IDetainedLicenseRepository detainedLicenseRepository)
        {
            RuleFor(x => x.LicenseId).ValidId();
            RuleFor(x => x.CreatedByUserID).ValidId();
            RuleFor(x => x).MustAsync(async (command, cancellation) => !await detainedLicenseRepository.IsLicenseDetained(command.LicenseId))
                .WithMessage("License is already detained");
        }
    }
}
