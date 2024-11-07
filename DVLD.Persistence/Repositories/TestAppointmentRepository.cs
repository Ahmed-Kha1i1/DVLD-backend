using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.TestAppointment.Common.Models;
using DVLD.Domain.Common.Enums;
using DVLD.Domain.Entities;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;
using Microsoft.Data.SqlClient;

namespace DVLD.Persistence.Repositories
{
    public class TestAppointmentRepository : GenericRepository<TestAppointment>, ITestAppointmentRepository
    {
        public TestAppointmentRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {
        }

        public override async Task<bool> DeleteAsync(int TestAppointmentId)
        {
            return await DeleteAsync("SP_DeleteTestAppointmentById", TestAppointmentId, "TestAppointmentID");
        }

        public async Task<TestAppointment?> GetByIdAsync(int TestAppointmentId)
        {
            return await GetEntityAsync<TestAppointment?>("SP_FindTestAppointmentById", TestAppointmentId, "TestAppointmentID");
        }

        public async Task<IReadOnlyList<TestAppointmentOverviewDTO>> ListPerTestTypeAsync(int LocalApplciationId, enTestType enTestType)
        {

            List<TestAppointmentOverviewDTO> list = new();

            await _dataSendhandler.Handle("SP_GetApplicationTestAppointmentsPerTestType", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@TestTypeID", (int)enTestType);
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalApplciationId);
                Connection.Open();
                using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        list.Add(_mapper.Map<TestAppointmentOverviewDTO>(reader));
                    }

                }
            });

            return list;
        }

        public async Task<bool> LockAppointment(int TestAppointmentID)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_LockAppointment", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                Connection.Open();
                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        protected override async Task<int?> AddAsync(TestAppointment entity)
        {
            int? TestAppointmentID = null;
            await _dataSendhandler.Handle("SP_AddNewTestAppointment", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@TestTypeID", entity.TestTypeID);
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", entity.LocalApplicationID);
                Command.Parameters.AddWithValue("@AppointmentDate", entity.AppointmentDate);
                Command.Parameters.AddWithValue("@PaidFees", entity.PaidFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);
                Command.Parameters.AddWithValue("@IsLocked", entity.IsLocked);
                if (entity.RetakeTestApplicationID == null)
                {
                    Command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@RetakeTestApplicationID", entity.RetakeTestApplicationID);
                }
                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestAppointmentID = insertedID;
                }
            });

            return TestAppointmentID;
        }

        protected override async Task<bool> UpdateAsync(TestAppointment entity)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_UpdateTestAppointment", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@TestAppointmentID", entity.Id);
                Command.Parameters.AddWithValue("@TestTypeID", entity.TestTypeID);
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", entity.LocalApplicationID);
                Command.Parameters.AddWithValue("@AppointmentDate", entity.AppointmentDate);
                Command.Parameters.AddWithValue("@PaidFees", entity.PaidFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);
                Command.Parameters.AddWithValue("@IsLocked", entity.IsLocked);

                if (entity.RetakeTestApplicationID == null)
                {
                    Command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@RetakeTestApplicationID", entity.RetakeTestApplicationID);
                }

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }
    }
}
