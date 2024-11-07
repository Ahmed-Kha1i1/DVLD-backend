using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.TestType.Common.Models;
using DVLD.Domain.Entities;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;

namespace DVLD.Persistence.Repositories
{
    public class TestTypeRepository : BaseRepository, ITestTypeRepository
    {

        public TestTypeRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {

        }

        public async Task<bool> DeleteAsync(int TestTypeId)
        {
            return await DeleteAsync("SP_DeleteTestTypeById", TestTypeId, "TestTypeId");
        }

        public async Task<TestType?> GetByIdAsync(int TestTypeId)
        {
            return await GetEntityAsync<TestType>("SP_FindTestTypeById", TestTypeId, "TestTypeID");
        }

        public async Task<IReadOnlyList<TestTypeDTO>> ListOverviewAsync()
        {
            return await ListAllAsync<TestTypeDTO>("SP_GetAllTestTypes");
        }

        public async Task<bool> SaveAsync(TestType entity)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_UpdateTestType", async (Connection, Command) =>
            {

                Connection.Open();
                Command.Parameters.AddWithValue("@TestTypeID", entity.Id);
                Command.Parameters.AddWithValue("@TestTypeTitle", entity.TestTypeTitle);
                Command.Parameters.AddWithValue("@TestTypeDescription", entity.TestTypeDescription);
                Command.Parameters.AddWithValue("@TestTypeFees", entity.TestTypeFees);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }
    }
}
