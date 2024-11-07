using DVLD.Application.Common.Response;
using DVLD.Application.Features.LocalApplication.Common.Models;
using MediatR;

namespace DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationQuery
{
    public class GetLocalApplicationQuery : IRequest<Response<LocalApplicationDTO>>
    {
        public int LocalApplicationId { get; set; }

        public GetLocalApplicationQuery(int localApplicationId)
        {
            LocalApplicationId = localApplicationId;
        }
    }
}
