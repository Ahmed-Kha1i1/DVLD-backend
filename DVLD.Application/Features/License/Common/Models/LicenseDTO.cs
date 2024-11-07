namespace DVLD.Application.Features.License.Common.Models
{
    public class LicenseDTO
    {
        public int LicenseId { get; set; }
        public int ApplicationId { get; set; }
        public string FullName { get; set; }
        public string ClassName { get; set; }
        public int LicenseClassId { get; set; }
        public string NationalNumber { get; set; }
        public string Gender { get; set; }
        public DateTime IssueDate { get; set; }
        public string IssueReason { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int DriverId { get; set; }
        public int PersonId { get; set; }
        public bool IsDetained { get; set; }
    }
}
