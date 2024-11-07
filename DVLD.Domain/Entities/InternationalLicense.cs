using DVLD.Domain.Common;

namespace DVLD.Domain.Entities
{
    public class InternationalLicense : BaseEntity
    {
        public int DriverID { get; set; }
        public Driver? DriverInfo;
        public int ApplicationID { get; set; }
        public Application? ApplicationInfo;
        public int IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedByUserID { get; set; }
    }
}
