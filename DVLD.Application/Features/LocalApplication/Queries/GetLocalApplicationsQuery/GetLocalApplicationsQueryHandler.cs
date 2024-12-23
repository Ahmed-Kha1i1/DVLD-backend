﻿
namespace DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationsQuery
{
    public class GetLocalApplicationsQueryHandler(ILocalApplicationRepository localApplicationRepository) : ResponseHandler, IRequestHandler<GetLocalApplicationsQuery, Response<GetLocalApplicationsQueryResponse>>
    {
        public async Task<Response<GetLocalApplicationsQueryResponse>> Handle(GetLocalApplicationsQuery request, CancellationToken cancellationToken)
        {
            var result = await localApplicationRepository.ListOverviewAsync(request);

            var resposne = new GetLocalApplicationsQueryResponse()
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
