namespace DataLayerCore.TestAppointment
{
    public class TestAppointmentFullDTO
    {
        public int TestAppointmentID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public string TestTypeTitle { get; set; }
        public string ClassName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public float PaidFees { get; set; }
        public string FullName { get; set; }
        public bool IsLocked { get; set; }

    }
}
