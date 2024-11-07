namespace DVLD.Application.Features.Application.Queries.GetApplicationQuery
{
    public class GetApplicationQuery : IRequest<Response<GetApplicationQueryResponse>>
    {
        public int applicationId { get; set; }

        public GetApplicationQuery(int applicationId)
        {
            this.applicationId = applicationId;
        }
    }
}
