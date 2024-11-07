using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Application.Features.Application.Common.Model;
using DVLD.Domain.Common.Enums;
using ApplicationEntity = DVLD.Domain.Entities;

namespace DVLD.Application.Contracts.Persistence
{
    public interface IApplicationRepository : IAsyncRepository<ApplicationEntity.Application>, IAsyncListOverviewRepository<ApplicationOverviewDTO>
    {
        Task<ApplicationOverviewDTO?> GetOverviewAsync(int ApplicationId);
        Task<int?> GetActiveApplicationId(int PersonID, int LicenseClassID);
        Task<bool> UpdateStatusAsync(int ApplicationId, enApplicationStatus ApplicationStatus);
        Task<bool> Cancel(int ApplicationId);
        Task<bool> SetComplete(int ApplicationId);
        Task<float?> GetpPaidFees(int ApplicationId);
    }
}

