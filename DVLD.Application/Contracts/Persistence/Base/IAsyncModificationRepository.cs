using DVLD.Domain.Common;

namespace DVLD.Application.Contracts.Persistence.Base
{
    public interface IAsyncModificationRepository<T> where T : BaseEntity
    {
        Task<bool> SaveAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
