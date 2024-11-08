namespace DVLD.Application.Features.Driver.Queries.GetDriversQuery
{
    public class GetDriversQueryHandler(IDriverRepository driverRepository)
        : ResponseHandler, IRequestHandler<GetDriversQuery, Response<GetDriversQueryResponse>>
    {
        public async Task<Response<GetDriversQueryResponse>> Handle(GetDriversQuery request, CancellationToken cancellationToken)
        {
            var result = await driverRepository.ListOverviewAsync(request);

            var resposne = new GetDriversQueryResponse()
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
