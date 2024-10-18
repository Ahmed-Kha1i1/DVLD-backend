using DVLD.Domain.Common;
using DVLD.Domain.Common.Enums;

namespace DVLD.Domain.Entities
{
    public class TestAppointment : BaseEntity
    {
        public enTestType TestTypeID { get; set; }
        public int LocalApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; } = DateTime.Now;
        public float PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; } = false;
        public int? RetakeTestApplicationID { get; set; }
        public Application? RetakeTestApplication { get; set; }
    }
}
