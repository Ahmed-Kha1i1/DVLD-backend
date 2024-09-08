namespace DataLayerCore.LocalDrivingLicenseApplication
{
    public class LocalDrivigFullData
    {
        public int LocalDrivingLicenseApplicationId { get; set; }
        public string ClassName { get; set; }
        public int NationalNo { get; set; }
        public string FullName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public byte PassedTestDate { get; set; }
        public string Status { get; set; }

    }
}
