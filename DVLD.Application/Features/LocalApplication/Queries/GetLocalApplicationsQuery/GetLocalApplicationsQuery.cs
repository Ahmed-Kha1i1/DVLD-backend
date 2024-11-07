using DVLD.Application.Common.Queries;

namespace DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationsQuery
{
    public class GetLocalApplicationsQuery : ItemsQueryBase, IRequest<Response<GetLocalApplicationsQueryResponse>>
    {
        public int? Id { get; set; } = null;
        public string? ClassName { get; set; } = null;
        public int? PersonId { get; set; } = null;
        public string? NationalNumber { get; set; } = null;
        public string? Status { get; set; } = null;
    }
}
