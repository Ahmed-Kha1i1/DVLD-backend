using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Application.Features.TestType.Common.Models;
using DVLD.Domain.Entities;

namespace DVLD.Application.Contracts.Persistence
{
    public interface ITestTypeRepository : IAsyncRepository<TestType>, IAsyncListOverviewRepository<TestTypeDTO>
    {
    }
}
