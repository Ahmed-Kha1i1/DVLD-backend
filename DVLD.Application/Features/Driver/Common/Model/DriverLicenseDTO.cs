namespace DVLD.Application.Features.Driver.Common.Model
{
    public class DriverLicenseDTO
    {
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public string ClassName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
