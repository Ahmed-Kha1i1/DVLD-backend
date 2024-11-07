using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Application.Features.LocalApplication.Common.Models;
using DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationPerTestTypeQuery;
using DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationsQuery;
using DVLD.Domain.Common.Enums;
using DVLD.Domain.Entities;

namespace DVLD.Application.Contracts.Persistence
{
    public interface ILocalApplicationRepository : IAsyncRepository<LocalApplication>, IAsyncListOverviewRepository<LocalApplicationOverviewDTO>
    {
        Task<(IReadOnlyList<LocalApplicationOverviewDTO> items, int TotalCount)> ListOverviewAsync(GetLocalApplicationsQuery request);
        Task<LocalApplicationDTO?> GetOverviewAsync(int LocalApplicationId);
        Task<LocalApplicationPrefDTO?> GetPref(int LocalApplicationId);
        Task<GetLocalApplicationPerTestTypeQueryResponse?> GetPerTestTypeAsync(int LocalApplicationId, enTestType TestTypeId);
        Task<int?> GetActiveLicenseID(int LocalApplicationId);
        Task<bool> DoesPassTestType(int LocalApplicationId, int TestTypeID);
        Task<bool> DoesPassPreviousTest(int LocalApplicationId, enTestType CurrentTestType);
        Task<bool> DoesAttendTestType(int LocalApplicationId, int TestTypeID);
        Task<bool> IsThereAnActiveScheduledTest(int LocalApplicationId, int TestTypeID);
    }
}
