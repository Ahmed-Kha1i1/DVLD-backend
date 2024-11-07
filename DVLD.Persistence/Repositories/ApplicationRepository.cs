using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.Application.Common.Model;
using DVLD.Domain.Common.Enums;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;
using ApplicationEntity = DVLD.Domain.Entities;

namespace DVLD.Persistence.Repositories
{
    public class ApplicationRepository : GenericRepository<ApplicationEntity.Application>, IApplicationRepository
    {
        public ApplicationRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {
        }

        public override async Task<bool> DeleteAsync(int ApplicationId)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_DeleteApplicationById", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationId);
                RowAffected = await Command.ExecuteNonQueryAsync();

            });

            return RowAffected > 0;
        }

        protected override async Task<int?> AddAsync(ApplicationEntity.Application entity)
        {
            int? ApplicationID = null;
            await _dataSendhandler.Handle("SP_AddNewApplication", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@ApplicantPersonID", entity.ApplicantPersonID);
                Command.Parameters.AddWithValue("@ApplicationDate", entity.ApplicationDate);
                Command.Parameters.AddWithValue("@ApplicationTypeID", entity.ApplicationTypeID);
                Command.Parameters.AddWithValue("@ApplicationStatus", entity.ApplicationStatusID);
                Command.Parameters.AddWithValue("@LastStatusDate", entity.LastStatusDate);
                Command.Parameters.AddWithValue("@PaidFees", entity.PaidFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);

                object? Result = await Command.ExecuteScalarAsync();

                if (Result is not null && int.TryParse(Result.ToString(), out int ID))
                {
                    ApplicationID = ID;
                }
            });


            return ApplicationID;
        }

        protected override async Task<bool> UpdateAsync(ApplicationEntity.Application entity)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_UpdateApplication", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@ApplicationID", entity.Id);
                Command.Parameters.AddWithValue("@ApplicantPersonID", entity.ApplicantPersonID);
                Command.Parameters.AddWithValue("@ApplicationDate", entity.ApplicationDate);
                Command.Parameters.AddWithValue("@ApplicationTypeID", entity.ApplicationTypeID);
                Command.Parameters.AddWithValue("@ApplicationStatus", entity.ApplicationStatusID);
                Command.Parameters.AddWithValue("@LastStatusDate", entity.LastStatusDate);
                Command.Parameters.AddWithValue("@PaidFees", entity.PaidFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        public async Task<int?> GetActiveApplicationId(int PersonID, int LicenseClassID)
        {
            int? ApplicationID = null;
            await _dataSendhandler.Handle("SP_GetActiveApplicationIDForLicenseClass", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                Command.Parameters.AddWithValue("@ApplicationTypeID", enApplicationStatus.New);
                Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);


                Connection.Open();
                object? Result = await Command.ExecuteScalarAsync();

                if (Result is not null && int.TryParse(Result.ToString(), out int ID))
                {
                    ApplicationID = ID;
                }
            });

            return ApplicationID;
        }

        public async Task<ApplicationOverviewDTO?> GetOverviewAsync(int ApplicationId)
        {
            return await GetAsync<ApplicationOverviewDTO>("SP_GetApplicationOverview", ApplicationId, "ApplicationId");
        }

        public async Task<IReadOnlyList<ApplicationOverviewDTO>> ListOverviewAsync()
        {
            return await ListAllAsync<ApplicationOverviewDTO>("SP_GetAllApplications");
        }

        public async Task<bool> UpdateStatusAsync(int ApplicationId, enApplicationStatus ApplicationStatus)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_UpdateStatus", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationId);
                Command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);
                Connection.Open();
                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        public async Task<bool> Cancel(int ApplicationId)
        {
            return await UpdateStatusAsync(ApplicationId, enApplicationStatus.Cancelled);
        }

        public async Task<bool> SetComplete(int ApplicationId)
        {
            return await UpdateStatusAsync(ApplicationId, enApplicationStatus.Completed);
        }

        public async Task<ApplicationEntity.Application?> GetByIdAsync(int ApplicationId)
        {
            return await GetEntityAsync<ApplicationEntity.Application>("SP_FindApplicationById", ApplicationId, "ApplicationID");
        }

        public async Task<float?> GetpPaidFees(int ApplicationId)
        {
            float? PaidFees = null;
            await _dataSendhandler.Handle("SP_GetPaidFees", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@ApplicationId", ApplicationId);
                Connection.Open();
                object? Result = await Command.ExecuteScalarAsync();

                if (Result is not null && float.TryParse(Result.ToString(), out float fees))
                {
                    PaidFees = fees;
                }
            });

            return PaidFees;
        }
    }
}
