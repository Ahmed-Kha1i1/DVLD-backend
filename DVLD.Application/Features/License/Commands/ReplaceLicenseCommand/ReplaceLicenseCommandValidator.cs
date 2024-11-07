using DVLD.Domain.Common.Enums;

namespace DVLD.Application.Features.License.Commands.ReplaceLicenseCommand
{
    public class ReplaceLicenseCommandValidator : AbstractValidator<ReplaceLicenseCommand>
    {
        public ReplaceLicenseCommandValidator()
        {
            RuleFor(license => license.OldLicenseId).ValidId();
            RuleFor(license => license.IssueReason)
               .Must(reason => reason == enIssueReason.ReplacementDamaged || reason == enIssueReason.ReplacementLost)
               .WithMessage("IssueReason must be either ReplacementDamaged or ReplacementLost.");
            RuleFor(license => license.CreatedUserId).ValidId();
        }
    }
}
