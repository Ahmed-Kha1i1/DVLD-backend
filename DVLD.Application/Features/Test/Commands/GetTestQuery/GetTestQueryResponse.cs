namespace DVLD.Application.Features.Test.Commands.GetTestQuery
{
    public class GetTestQueryResponse
    {
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string? Notes { get; set; }
        public int CreatedByUserID { get; set; }
    }
}
