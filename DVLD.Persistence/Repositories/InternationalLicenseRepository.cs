using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.InternationalLicense.Common.Model;
using DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicenseQuery;
using DVLD.Domain.Entities;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;

namespace DVLD.Persistence.Repositories
{
    public class InternationalLicenseRepository : GenericRepository<InternationalLicense>, IInternationalLicenseRepository
    {
        public InternationalLicenseRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {
        }

        public override async Task<bool> DeleteAsync(int internationalLicenseId)
        {
            return await DeleteAsync("SP_DeleteinternationalLicenseById", internationalLicenseId, "internationalLicenseId");
        }

        public async Task<int?> GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int? InternationalLicenseID = null;
            await _dataSendhandler.Handle("SP_GetActiveInternationalLicenseIDByDriverID", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@DriverID", DriverID);
                Connection.Open();

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                }
            });


            return InternationalLicenseID;
        }

        public async Task<InternationalLicense?> GetByIdAsync(int internationalLicenseId)
        {
            return await GetEntityAsync<InternationalLicense>("SP_FindInternationalLicensById", internationalLicenseId, "InternationalLicenseID");
        }

        public async Task<GetInternationalLicenseQueryResponse?> GetInternationalLicense(int internationalLicenseId)
        {
            return await GetAsync<GetInternationalLicenseQueryResponse>("SP_GetInternationalLicenseDetails", internationalLicenseId, "InternationalLicenseID");
        }

        public async Task<IReadOnlyList<InternationalLicenseOverviewDTO>> ListOverviewAsync()
        {
            return await ListAllAsync<InternationalLicenseOverviewDTO>("SP_GetAllInternationalLicenses");
        }

        protected override async Task<int?> AddAsync(InternationalLicense entity)
        {
            int? InternationalLicenseID = null;
            await _dataSendhandler.Handle("SP_AddNewInternationalLicens", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@ApplicationID", entity.ApplicationID);
                Command.Parameters.AddWithValue("@DriverID", entity.DriverID);
                Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", entity.IssuedUsingLocalLicenseID);
                Command.Parameters.AddWithValue("@IssueDate", entity.IssueDate);
                Command.Parameters.AddWithValue("@ExpirationDate", entity.ExpirationDate);
                Command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                Command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                }
            });

            return InternationalLicenseID;
        }

        protected override async Task<bool> UpdateAsync(InternationalLicense entity)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_UpdateInternationalLicens", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@InternationalLicenseID", entity.Id);
                Command.Parameters.AddWithValue("@ApplicationID", entity.ApplicationID);
                Command.Parameters.AddWithValue("@DriverID", entity.DriverID);
                Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", entity.IssuedUsingLocalLicenseID);
                Command.Parameters.AddWithValue("@IssueDate", entity.IssueDate);
                Command.Parameters.AddWithValue("@ExpirationDate", entity.ExpirationDate);
                Command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                Command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }
    }
}
