namespace DVLD.Application.Features.Test.Commands.AddTestCommand
{
    public class AddTestCommand : IRequest<Response<int?>>
    {
        public int TestAppointmentId { get; set; }
        public bool Result { get; set; }
        public string? Notes { get; set; }
        public int CreatedUserId { get; set; }
    }
}
