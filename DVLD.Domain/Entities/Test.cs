using DVLD.Domain.Common;

namespace DVLD.Domain.Entities
{
    public class Test : BaseEntity
    {
        public int TestAppointmentID { get; set; }
        public TestAppointment? TestAppointmentInfo { get; set; }
        public bool TestResult { get; set; }
        public string? Notes { get; set; }
        public int CreatedByUserID { get; set; }
    }
}
