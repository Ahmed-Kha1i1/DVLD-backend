using DVLD.Domain.Common.Enums;

namespace DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationPerTestTypeQuery
{
    public class GetLocalApplicationPerTestTypeQuery : IRequest<Response<GetLocalApplicationPerTestTypeQueryResponse>>
    {
        public int LocalApplicationId { get; set; }
        public enTestType TestTypeId { get; set; }

    }
}
