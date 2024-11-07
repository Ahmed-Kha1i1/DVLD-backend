using DVLD.Domain.Common.Enums;

namespace DVLD.Application.Features.License.Commands.ReplaceLicenseCommand
{
    public class ReplaceLicenseCommand : IRequest<Response<int?>>
    {
        public int OldLicenseId { get; set; }
        public enIssueReason IssueReason { get; set; }
        public int CreatedUserId { get; set; }
    }
}
