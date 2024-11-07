namespace DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicenseQuery
{
    public class GetDetainedLicenseQueryResponse
    {
        public int DetainedId { set; get; }
        public int LicenseID { set; get; }
        public DateTime DetainDate { set; get; }
        public float FineFees { set; get; }
        public int CreatedByUserID { set; get; }
        public bool IsReleased { set; get; }
        public DateTime? ReleaseDate { set; get; }
        public int? ReleasedByUserID { set; get; }
        public int? ReleaseApplicationID { set; get; }
    }
}
