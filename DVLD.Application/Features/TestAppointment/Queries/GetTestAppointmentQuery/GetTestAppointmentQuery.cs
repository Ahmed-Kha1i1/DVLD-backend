namespace DVLD.Application.Features.TestAppointment.Queries.GetTestAppointmentQuery
{
    public class GetTestAppointmentQuery : IRequest<Response<GetTestAppointmentQueryResponse>>
    {
        public int TestAppointmentId { get; set; }

        public GetTestAppointmentQuery(int testAppointmentId)
        {
            TestAppointmentId = testAppointmentId;
        }
    }
}
