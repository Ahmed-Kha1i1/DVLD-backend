namespace DVLD.Application.Features.License.Commands.RenewLicenseCommand
{
    public class RenewLicenseCommandValidator : AbstractValidator<RenewLicenseCommand>
    {
        public RenewLicenseCommandValidator()
        {
            RuleFor(x => x.OldLicenseId).ValidId();
            RuleFor(x => x.CreatedUserId).ValidId();
        }
    }
}
