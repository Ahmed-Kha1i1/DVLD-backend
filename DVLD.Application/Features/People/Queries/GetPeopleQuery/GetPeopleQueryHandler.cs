namespace DVLD.Application.Features.People.Queries.GetPeopleQuery
{
    public class GetPeopleQueryHandler(IPersonRepository personRepository)
        : ResponseHandler, IRequestHandler<GetPeopleQuery, Response<GetPeopleQueryResponse>>
    {
        public async Task<Response<GetPeopleQueryResponse>> Handle(GetPeopleQuery request, CancellationToken cancellationToken)
        {

            var result = await personRepository.ListOverviewAsync(request);

            var resposne = new GetPeopleQueryResponse()
            {
                Items = result.items,
                Metadata = new()
                {
                    TotalCount = result.totalCount,
                    PageSize = request.PageSize,
                    PageNumber = request.PageNumber
                }
            };
            return Success(resposne);
        }
    }
}
