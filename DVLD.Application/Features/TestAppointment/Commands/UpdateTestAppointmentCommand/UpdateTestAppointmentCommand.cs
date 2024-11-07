namespace DVLD.Application.Features.TestAppointment.Commands.UpdateTestAppointmentCommand
{
    public class UpdateTestAppointmentCommand : IRequest<Response<bool>>
    {
        public int TestAppointmentId { get; set; }
        public int CreatedUserId { get; set; }
        public DateOnly AppointmentDate { get; set; }
    }
}
