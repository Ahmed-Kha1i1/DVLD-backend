using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.License.Common.Models;
using DVLD.Domain.Entities;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;


namespace DVLD.Persistence.Repositories
{
    public class LicenseRepository : GenericRepository<License>, ILicenseRepository
    {
        public LicenseRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {
        }

        public async Task<LicenseDTO?> GetAsync(int LicenseId)
        {
            return await GetAsync<LicenseDTO>("SP_GetLicenseSummary", LicenseId, "LicenseId");
        }

        public async Task<int?> GetActiveLicenseId(int PersonId, int LicenseClassId)
        {
            int? LicenseID = null;
            await _dataSendhandler.Handle("SP_FindActiveLicenseIDByPersonID", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@PersonID", PersonId);
                Command.Parameters.AddWithValue("@LicenseClass", LicenseClassId);


                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int Id))
                {
                    LicenseID = Id;

                }
            });

            return LicenseID;
        }

        public async Task<bool> IsLicenseExist(int PersonId, int LicenseClassId)
        {
            return await GetActiveLicenseId(PersonId, LicenseClassId) != null;
        }

        public async Task<bool> DeactivateLicense(int LicenseId)
        {

            int rowsAffected = 0;
            await _dataSendhandler.Handle("SP_DeactivateLicense", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@LicenseID", LicenseId);
                rowsAffected = await Command.ExecuteNonQueryAsync();
            });

            return rowsAffected > 0;
        }

        protected override async Task<int?> AddAsync(License entity)
        {
            int? LicenseID = null;
            await _dataSendhandler.Handle("SP_AddNewLicens", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@ApplicationID", entity.ApplicationID);
                Command.Parameters.AddWithValue("@DriverID", entity.DriverID);
                Command.Parameters.AddWithValue("@LicenseClass", entity.LicenseClassID);
                Command.Parameters.AddWithValue("@IssueDate", entity.IssueDate);
                Command.Parameters.AddWithValue("@ExpirationDate", entity.ExpirationDate);
                if (string.IsNullOrWhiteSpace(entity.Notes))
                {
                    Command.Parameters.AddWithValue("@Notes", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@Notes", entity.Notes);
                }
                Command.Parameters.AddWithValue("@PaidFees", entity.PaidFees);
                Command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                Command.Parameters.AddWithValue("@IssueReason", entity.IssueReason);
                Command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }

            });

            return LicenseID;
        }

        protected override async Task<bool> UpdateAsync(License entity)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_UpdateLicens", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@LicenseID", entity.Id);
                Command.Parameters.AddWithValue("@ApplicationID", entity.ApplicationID);
                Command.Parameters.AddWithValue("@DriverID", entity.DriverID);
                Command.Parameters.AddWithValue("@entityClassID", entity.LicenseClassID);
                Command.Parameters.AddWithValue("@IssueDate", entity.IssueDate);
                Command.Parameters.AddWithValue("@ExpirationDate", entity.ExpirationDate);
                if (string.IsNullOrWhiteSpace(entity.Notes))
                {
                    Command.Parameters.AddWithValue("@Notes", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@Notes", entity.Notes);
                }
                Command.Parameters.AddWithValue("@PaidFees", entity.PaidFees);
                Command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                Command.Parameters.AddWithValue("@IssueReason", entity.IssueReason);
                Command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        public override async Task<bool> DeleteAsync(int licenseId)
        {
            return await DeleteAsync("SP_DeleteLicense", licenseId, "LicenseID");
        }

        public async Task<License?> GetByIdAsync(int licenseId)
        {
            return await GetEntityAsync<License>("SP_FindLicensById", licenseId, "LicenseID");
        }

        public async Task<int?> GetPersonId(int LicenseId)
        {
            int? PersonId = null;
            await _dataSendhandler.Handle("SP_GetPersonIdBylicenseId", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@LicenseId", LicenseId);
                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int Id))
                {
                    PersonId = Id;

                }
            });

            return PersonId;
        }
    }
}
