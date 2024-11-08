using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Application.Features.DetainedLicense.Common.Models;
using DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicensesQuery;
using DVLD.Domain.Entities;

namespace DVLD.Application.Contracts.Persistence
{
    public interface IDetainedLicenseRepository : IAsyncModificationRepository<DetainedLicense>, IAsyncListOverviewRepository<DetainedLicenseOverviewDTO>, IAsyncGetRepository<DetainedLicense>
    {
        Task<DetainedLicense?> GetByLicenseIdAsync(int LicenseId);
        Task<bool> ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID);
        Task<bool> IsLicenseDetained(int LicenseID);
        Task<(IReadOnlyList<DetainedLicenseOverviewDTO> items, int totalCount)> ListOverviewAsync(GetDetainedLicensesQuery request);
    }
}
