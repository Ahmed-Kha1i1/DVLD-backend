using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Application.Features.InternationalLicense.Common.Model;
using DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicenseQuery;
using DVLD.Domain.Entities;

namespace DVLD.Application.Contracts.Persistence
{
    public interface IInternationalLicenseRepository : IAsyncModificationRepository<InternationalLicense>, IAsyncListOverviewRepository<InternationalLicenseOverviewDTO>, IAsyncGetRepository<InternationalLicense>
    {
        Task<int?> GetActiveInternationalLicenseIDByDriverID(int DriverID);
        Task<GetInternationalLicenseQueryResponse?> GetInternationalLicense(int internationalLicenseId);
    }
}
