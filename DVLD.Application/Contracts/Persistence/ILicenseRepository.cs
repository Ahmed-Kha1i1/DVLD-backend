using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Application.Features.License.Common.Models;
using DVLD.Domain.Entities;

namespace DVLD.Application.Contracts.Persistence
{
    public interface ILicenseRepository : IAsyncModificationRepository<License>, IAsyncGetRepository<License>
    {
        Task<LicenseDTO?> GetAsync(int LicenseId);
        Task<int?> GetActiveLicenseId(int PersonId, int LicenseClassId);
        Task<int?> GetPersonId(int LicenseId);
        Task<bool> IsLicenseExist(int PersonID, int LicenseClassID);
        Task<bool> DeactivateLicense(int LicenseId);
    }
}
