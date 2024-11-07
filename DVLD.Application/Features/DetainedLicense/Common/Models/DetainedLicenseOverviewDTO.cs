namespace DVLD.Application.Features.DetainedLicense.Common.Models
{
    public class DetainedLicenseOverviewDTO
    {
        public int DetainID { get; set; }
        public int PersonId { get; set; }
        public int DriverId { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public float FineFees { get; set; }
        public bool IsReleased { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string NationalNo { get; set; }
        public string FullName { get; set; }
        public int? ReleaseApplicationID { get; set; }
    }
}
