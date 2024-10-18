using DVLD.Domain.Common;

namespace DVLD.Application.Contracts.Persistence.Base
{
    public interface IAsyncGetRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
    }
}
