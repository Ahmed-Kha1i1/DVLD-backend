using DVLD.Domain.Common;
using DVLD.Domain.Common.Enums;

namespace DVLD.Domain.Entities
{
    public class License : BaseEntity
    {
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public Driver? DriverInfo;
        public int LicenseClassID { get; set; }
        public LicenseClass? LicenseClassInfo;
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime ExpirationDate { get; set; }
        public string? Notes { get; set; }
        public float PaidFees { get; set; }
        public bool IsActive { get; set; } = true;
        public enIssueReason IssueReason { get; set; } = enIssueReason.FirstTime;
        public int CreatedByUserID { get; set; }
    }
}
