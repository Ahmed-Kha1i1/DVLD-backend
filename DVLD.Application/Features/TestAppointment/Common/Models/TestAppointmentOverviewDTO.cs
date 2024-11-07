namespace DVLD.Application.Features.TestAppointment.Common.Models
{
    public class TestAppointmentOverviewDTO
    {
        public int TestAppointmentId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public float PaidFees { get; set; }
        public bool IsLocked { get; set; }
    }
}
