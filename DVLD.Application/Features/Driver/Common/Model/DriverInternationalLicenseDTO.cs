namespace DVLD.Application.Features.Driver.Common.Model
{
    public class DriverInternationalLicenseDTO
    {
        public int InternationalLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
