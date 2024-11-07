using DVLD.Application.Features.LocalApplication.Common.Models;

namespace DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationQuery
{
    public class GetLocalApplicationPrefQuery : IRequest<Response<LocalApplicationPrefDTO>>
    {
        public int LocalApplicationId { get; set; }

        public GetLocalApplicationPrefQuery(int localApplicationId)
        {
            LocalApplicationId = localApplicationId;
        }
    }
}
