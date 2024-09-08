namespace DataLayerCore.DetainedLicense
{
    public abstract class DetainedLicenseForModificationDTO
    {
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public float FineFees { get; set; }
        public int CreatedByUserID { get; set; }


    }
}
