using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Application.Features.Driver.Common.Model;
using DVLD.Application.Features.Driver.Queries.GetDriversQuery;
using DVLD.Domain.Entities;

namespace DVLD.Application.Contracts.Persistence
{
    public interface IDriverRepository : IAsyncModificationRepository<Driver>, IAsyncListOverviewRepository<DriverOverviewDTO>, IAsyncGetRepository<Driver>
    {
        Task<Person?> GetPerson(int driverId);
        Task<DriverDTO?> GetFullAsync(int driverId);
        Task<DriverDTO?> GetBypersonId(int personId);
        Task<IReadOnlyList<DriverLicenseDTO>> ListDriverLicensesAsync(int driverId);
        Task<IReadOnlyList<DriverInternationalLicenseDTO>> ListDriverInternationalLicensesAsync(int driverId);
        Task<(IReadOnlyList<DriverOverviewDTO> items, int totalCount)> ListOverviewAsync(GetDriversQuery request);
    }
}
