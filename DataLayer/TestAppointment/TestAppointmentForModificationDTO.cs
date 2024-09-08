using DataLayerCore.TestType;

namespace DataLayerCore.TestAppointment
{
    public abstract class TestAppointmentForModificationDTO
    {
        public enTestType TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public float PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int? RetakeTestApplicationID { get; set; }

    }
}
