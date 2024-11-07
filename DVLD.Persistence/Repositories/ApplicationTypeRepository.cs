using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.ApplicationType.Common.Models;
using DVLD.Domain.Entities;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;

namespace DVLD.Persistence.Repositories
{
    public class ApplicationTypeRepository : BaseRepository, IApplicationTypeRepository
    {
        public ApplicationTypeRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {
        }

        public async Task<bool> DeleteAsync(int ApplicationTypeId)
        {
            return await DeleteAsync("SP_DeleteApplicationTypeById", ApplicationTypeId, "ApplicationTypeId");
        }

        public async Task<ApplicationType?> GetByIdAsync(int ApplicationTypeId)
        {
            return await GetEntityAsync<ApplicationType>("SP_FindApplicationTypeById", ApplicationTypeId, "ApplicationTypeId");
        }

        public async Task<IReadOnlyList<ApplicationTypeDTO>> ListOverviewAsync()
        {
            return await ListAllAsync<ApplicationTypeDTO>("SP_GetAllApplicationTypes");
        }

        public async Task<bool> SaveAsync(ApplicationType entity)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_UpdateApplicationType", async (Connection, Command) =>
            {

                Connection.Open();
                Command.Parameters.AddWithValue("@ApplicationTypeId", entity.Id);
                Command.Parameters.AddWithValue("@ApplicationTypeTitle", entity.ApplicationTypeTitle);
                Command.Parameters.AddWithValue("@ApplicationFees", entity.ApplicationFees);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }
    }
}
