namespace DVLD.Application.Features.InternationalLicense.Common.Model
{
    public class InternationalLicenseOverviewDTO
    {
        public int InternationalLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int PersonId { get; set; }
        public int DriverID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserID { get; set; }
    }
}
