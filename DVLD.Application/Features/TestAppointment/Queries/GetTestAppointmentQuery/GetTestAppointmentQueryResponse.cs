using DVLD.Application.Features.TestAppointment.Common.Models;
using DVLD.Domain.Common.Enums;

namespace DVLD.Application.Features.TestAppointment.Queries.GetTestAppointmentQuery
{
    public class GetTestAppointmentQueryResponse
    {
        public int TestAppointmentId { get; set; }
        public enTestType TestTypeID { get; set; }
        public int LocalApplicationID { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public float PaidFees { get; set; }
        public bool IsLocked { get; set; }
        public int? RetakeTestApplicationID { get; set; }
        public RetakeTestApplicationDTO? RetakeTestApplication { get; set; }
    }
}
