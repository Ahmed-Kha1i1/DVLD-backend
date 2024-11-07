namespace DVLD.Application.Features.Test.Commands.GetTestQuery
{
    public class GetTestQuery : IRequest<Response<GetTestQueryResponse>>
    {
        public int TestId { get; set; }

        public GetTestQuery(int testId)
        {
            TestId = testId;
        }
    }
}
