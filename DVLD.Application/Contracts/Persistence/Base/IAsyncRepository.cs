using DVLD.Domain.Common;

namespace DVLD.Application.Contracts.Persistence.Base
{
    public interface IAsyncRepository<T> : IAsyncGetRepository<T>, IAsyncModificationRepository<T> where T : BaseEntity
    {

    }
}
