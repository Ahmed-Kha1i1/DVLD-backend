using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Domain.Entities;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;

namespace DVLD.Persistence.Repositories
{
    public class TestRepository : GenericRepository<Test>, ITestRepository
    {
        public TestRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {
        }

        public override async Task<bool> DeleteAsync(int TestId)
        {
            return await DeleteAsync("SP_DeleteTestById", TestId, "TestId");
        }

        public async Task<Test?> GetByIdAsync(int testId)
        {
            return await GetEntityAsync<Test>("SP_FindTestById", testId, "TestID");
        }

        public async Task<int?> GetTestpassCount(int LocalApplicationID)
        {
            byte PassedTestCount = 0;
            await _dataSendhandler.Handle("SP_GetTestpassCount", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalApplicationID);

                Connection.Open();

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && byte.TryParse(result.ToString(), out byte ptCount))
                {
                    PassedTestCount = ptCount;
                }
            });

            return PassedTestCount;
        }

        public async Task<bool> PassedAllTests(int LocalApplicationID)
        {
            return await GetTestpassCount(LocalApplicationID) == 3;
        }

        protected override async Task<int?> AddAsync(Test entity)
        {
            int? TestID = null;
            await _dataSendhandler.Handle("SP_AddNewTest", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@TestAppointmentID", entity.TestAppointmentID);
                Command.Parameters.AddWithValue("@TestResult", entity.TestResult);
                if (string.IsNullOrWhiteSpace(entity.Notes))
                {
                    Command.Parameters.AddWithValue("@Notes", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@Notes", entity.Notes);
                }
                Command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
            });

            return TestID;
        }

        protected override async Task<bool> UpdateAsync(Test entity)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_UpdateTest", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@TestID", entity.Id);
                Command.Parameters.AddWithValue("@TestAppointmentID", entity.TestAppointmentID);
                Command.Parameters.AddWithValue("@TestResult", entity.TestResult);
                if (string.IsNullOrWhiteSpace(entity.Notes))
                {
                    Command.Parameters.AddWithValue("@Notes", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@Notes", entity.Notes);
                }
                Command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });


            return RowAffected > 0;

        }
    }
}
