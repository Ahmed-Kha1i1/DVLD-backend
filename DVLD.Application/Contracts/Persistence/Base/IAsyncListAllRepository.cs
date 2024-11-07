
using DVLD.Domain.Common;

namespace DVLD.Application.Contracts.Persistence.Base
{
    public interface IAsyncListAllRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> ListAllAsync();
    }

}