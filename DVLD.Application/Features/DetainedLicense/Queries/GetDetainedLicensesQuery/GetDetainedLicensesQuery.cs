using DVLD.Application.Common.Queries;

namespace DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicensesQuery
{
    public class GetDetainedLicensesQuery : ItemsQueryBase, IRequest<Response<GetDetainedLicensesQueryResponse>>
    {
        public int? Id { get; set; } = null;
        public string? NationalNumber { get; set; } = null;
        public bool? IsReleased { get; set; } = null;
    }
}
