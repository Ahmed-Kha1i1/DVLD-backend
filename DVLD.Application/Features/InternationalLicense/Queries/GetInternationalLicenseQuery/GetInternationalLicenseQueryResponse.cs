namespace DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicenseQuery
{
    public class GetInternationalLicenseQueryResponse
    {
        public string FullName { get; set; }
        public int InternationalLicenseId { get; set; }
        public int ApplicationId { get; set; }
        public int LocalLicenseId { get; set; }
        public bool IsActive { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string NationalNumber { get; set; }
        public string Gender { get; set; }
        public int DriverId { get; set; }
    }
}
