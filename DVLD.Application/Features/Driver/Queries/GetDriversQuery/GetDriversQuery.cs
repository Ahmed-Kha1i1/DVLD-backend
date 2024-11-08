using DVLD.Application.Common.Queries;

namespace DVLD.Application.Features.Driver.Queries.GetDriversQuery
{
    public class GetDriversQuery : ItemsQueryBase, IRequest<Response<GetDriversQueryResponse>>
    {
        public int? Id { get; set; } = null;
        public int? PersonId { get; set; } = null;
        public string? NationalNumber { get; set; } = null;
    }
}
