using DVLD.Domain.Common.Enums;

namespace DVLD.Application.Features.TestAppointment.Commands.AddTestAppointmentCommand
{
    public class AddTestAppointmentCommand : IRequest<Response<int?>>
    {
        public int LocalApplicationId { get; set; }
        public enTestType TestTypeId { get; set; }
        public int CreatedUserId { get; set; }
        public DateOnly AppointmentDate { get; set; }
    }
}
