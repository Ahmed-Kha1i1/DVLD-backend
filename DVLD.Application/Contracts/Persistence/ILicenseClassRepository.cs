using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Domain.Entities;

namespace DVLD.Application.Contracts.Persistence
{
    public interface ILicenseClassRepository : IAsyncGetRepository<LicenseClass>, IAsyncListAllRepository<LicenseClass>
    {
        Task<bool> IsPersonAgeValidForLicenseAsync(int personId, int licenseClassId);
    }
}
