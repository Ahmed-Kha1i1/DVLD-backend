namespace DVLD.Application.Features.Users.Queries.GetUsersQuery
{
    public class GetUsersQueryHandler(IUserRepository userRepository)
        : ResponseHandler, IRequestHandler<GetUsersQuery, Response<GetUsersQueryResponse>>
    {
        public async Task<Response<GetUsersQueryResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await userRepository.ListOverviewAsync(request);

            var resposne = new GetUsersQueryResponse()
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
