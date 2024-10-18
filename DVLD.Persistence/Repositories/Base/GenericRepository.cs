using AutoMapper;
using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Domain.Common;
using DVLD.Domain.Common.Enums;
using DVLD.Persistence.Handlers;

namespace DVLD.Persistence.Repositories.Base
{
    public abstract class GenericRepository<IEntity> : BaseRepository, IAsyncModificationRepository<IEntity> where IEntity : BaseEntity
    {
        protected GenericRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {
        }

        protected abstract Task<int?> AddAsync(IEntity entity);
        protected abstract Task<bool> UpdateAsync(IEntity entity);
        public abstract Task<bool> DeleteAsync(int id);
        public async Task<bool> SaveAsync(IEntity entity)
        {
            switch (entity.Mode)
            {
                case enMode.AddNew:
                    int? id = await AddAsync(entity);

                    if (id is not null)
                    {
                        entity.Id = id ?? default;
                        entity.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return await UpdateAsync(entity);
            }

            return false;
        }


    }
}
