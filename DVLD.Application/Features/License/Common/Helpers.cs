using DVLD.Domain.Common.Enums;

namespace DVLD.Application.Features.License.Common
{
    public static class Helpers
    {
        public static string GetIssueReasonText(enIssueReason IssueReason)
        {

            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.ReplacementDamaged:
                    return "Replacement for Damaged";
                case enIssueReason.ReplacementLost:
                    return "Replacement for Lost";
                default:
                    return "Unknown Resean";
            }
        }
    }
}
