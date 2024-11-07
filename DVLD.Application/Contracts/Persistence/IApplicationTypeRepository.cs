using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Application.Features.ApplicationType.Common.Models;
using DVLD.Domain.Entities;

namespace DVLD.Application.Contracts.Persistence
{
    public interface IApplicationTypeRepository : IAsyncRepository<ApplicationType>, IAsyncListOverviewRepository<ApplicationTypeDTO>
    {
    }
}
