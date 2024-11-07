namespace DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicenseApplicationQuery
{
    public class GetInternationalLicenseApplicationQueryResponse
    {
        public int ApplicationId { get; set; }
        public float ApplicationFees { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int LocalLicenseId { get; set; }
        public int internationalLicenseId { get; set; }
        public int CreatedUserId { get; set; }
    }
}
