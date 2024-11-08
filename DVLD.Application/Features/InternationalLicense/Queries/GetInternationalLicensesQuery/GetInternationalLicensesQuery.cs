using DVLD.Application.Common.Queries;

namespace DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicensesQuery
{
    public class GetInternationalLicensesQuery : ItemsQueryBase, IRequest<Response<GetInternationalLicensesQueryResposne>>
    {
        public int? Id { get; set; } = null;
        public int? DriverId { get; set; } = null;
        public int? LicenseId { get; set; } = null;
        public bool? IsActive { get; set; } = null;

    }
}
