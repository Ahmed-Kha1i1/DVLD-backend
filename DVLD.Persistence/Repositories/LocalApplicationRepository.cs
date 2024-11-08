using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.LocalApplication.Common.Models;
using DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationPerTestTypeQuery;
using DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationsQuery;
using DVLD.Domain.Common.Enums;
using DVLD.Domain.Entities;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DVLD.Persistence.Repositories
{
    public class LocalApplicationRepository : GenericRepository<LocalApplication>, ILocalApplicationRepository
    {
        public LocalApplicationRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {
        }

        public override async Task<bool> DeleteAsync(int LocalApplicationId)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_DeleteLocalDrivingLicenseApplicationById", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalApplicationId);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        public async Task<bool> DoesAttendTestType(int LocalApplicationId, int TestTypeID)
        {
            bool Result = false;
            await _dataSendhandler.Handle("SP_DoesAttendTestType", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalApplicationId);
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                Connection.Open();

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null)
                {
                    Result = true;
                }
            });


            return Result;
        }

        public async Task<bool> DoesPassTestType(int LocalApplicationId, int TestTypeID)
        {
            bool Result = false;
            await _dataSendhandler.Handle("SP_DoesPassTestType", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalApplicationId);
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                Connection.Open();

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && bool.TryParse(result.ToString(), out bool returnedResult))
                {
                    Result = returnedResult;
                }
            });

            return Result;
        }

        public async Task<int?> GetActiveLicenseID(int LocalApplicationId)
        {
            int? LicenseID = null;
            await _dataSendhandler.Handle("SP_FindActiveLicenseIDByApplication", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@LocalApplicationId", LocalApplicationId);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;

                }
            });

            return LicenseID;
        }

        public async Task<LocalApplication?> GetByIdAsync(int LocalApplicationId)
        {
            return await GetEntityAsync<LocalApplication>("SP_FindLocalDrivingLicenseApplicationById", LocalApplicationId, "LocalApplicationId");
        }

        public async Task<LocalApplicationDTO?> GetOverviewAsync(int LocalApplicationId)
        {
            return await GetAsync<LocalApplicationDTO>("SP_GetLocalApplicationOverview", LocalApplicationId, "LocalApplicationId");
        }

        public async Task<LocalApplicationPrefDTO?> GetPref(int LocalApplicationId)
        {
            return await GetAsync<LocalApplicationPrefDTO>("SP_GetLocalApplciationPref", LocalApplicationId, "localApplicationId");
        }

        public async Task<bool> IsThereAnActiveScheduledTest(int LocalApplicationId, int TestTypeID)
        {
            bool Result = false;
            await _dataSendhandler.Handle("SP_IsThereAnActiveScheduledTest", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalApplicationId);
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                Connection.Open();

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null)
                {
                    Result = true;
                }
            });

            return Result;
        }

        public async Task<IReadOnlyList<LocalApplicationOverviewDTO>> ListOverviewAsync()
        {
            return await ListAllAsync<LocalApplicationOverviewDTO>("SP_GetAllLocalDrivingLicenseApplications");
        }

        public async Task<(IReadOnlyList<LocalApplicationOverviewDTO> items, int totalCount)> ListOverviewAsync(GetLocalApplicationsQuery request)
        {
            List<LocalApplicationOverviewDTO> Items = new();

            int TotalCount = 0;
            await _dataSendhandler.Handle("SP_GetAllLocalDrivingLicenseApplications", async (Connection, Command) =>
            {
                var TotalCountParameter = new SqlParameter("@TotalCount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                Command.Parameters.AddWithValue("@SearchQuery", request.SearchQuery);
                Command.Parameters.AddWithValue("@Id", request.Id);
                Command.Parameters.AddWithValue("@ClassName", request.ClassName);
                Command.Parameters.AddWithValue("@PersonId", request.PersonId);
                Command.Parameters.AddWithValue("@NationalNumber", request.NationalNumber);
                Command.Parameters.AddWithValue("@Status", request.Status);
                Command.Parameters.AddWithValue("@OrderBy", request.OrderBy);
                Command.Parameters.AddWithValue("@OrderDirection", request.OrderDirection);
                Command.Parameters.AddWithValue("@PageNumber", request.PageNumber);
                Command.Parameters.AddWithValue("@PageSize", request.PageSize);
                Command.Parameters.Add(TotalCountParameter);

                Connection.Open();
                using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        Items.Add(_mapper.Map<LocalApplicationOverviewDTO>(reader));
                    }

                }
                TotalCount = (int?)TotalCountParameter.Value ?? 0;
            });

            return (Items, TotalCount);
        }

        protected override async Task<int?> AddAsync(LocalApplication entity)
        {
            int? LocalDrivingLicenseApplicationID = null;
            await _dataSendhandler.Handle("SP_AddNewLocalDrivingLicenseApplication", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@ApplicationID", entity.ApplicationId);
                Command.Parameters.AddWithValue("@LicenseClassID", entity.LicenseClassID);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LocalDrivingLicenseApplicationID = insertedID;
                }
            });

            return LocalDrivingLicenseApplicationID;
        }

        protected override async Task<bool> UpdateAsync(LocalApplication entity)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_UpdateLocalDrivingLicenseApplication", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", entity.Id);
                Command.Parameters.AddWithValue("@ApplicationID", entity.ApplicationId);
                Command.Parameters.AddWithValue("@LicenseClassID", entity.LicenseClassID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        public async Task<bool> DoesPassPreviousTest(int LocalApplicationId, enTestType CurrentTestType)
        {

            switch (CurrentTestType)
            {
                case enTestType.VisionTest:
                    //in this case no required prvious test to pass.
                    return true;

                case enTestType.WrittenTest:
                    //Written Test, you cannot sechdule it before person passes the vision test.
                    //we check if pass visiontest 1.

                    return await DoesPassTestType(LocalApplicationId, (int)enTestType.VisionTest);


                case enTestType.StreetTest:

                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    return await DoesPassTestType(LocalApplicationId, (int)enTestType.WrittenTest);

                default:
                    return false;
            }
        }

        public async Task<GetLocalApplicationPerTestTypeQueryResponse?> GetPerTestTypeAsync(int LocalApplicationId, enTestType TestTypeId)
        {
            GetLocalApplicationPerTestTypeQueryResponse? entity = null;

            await _dataSendhandler.Handle("SP_GetLocalApplicationPerTestType", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue($"@LocalApplicationId", LocalApplicationId);
                Command.Parameters.AddWithValue($"@TestTypeId", TestTypeId);
                Connection.Open();
                using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        entity = _mapper.Map<GetLocalApplicationPerTestTypeQueryResponse>(reader);
                    }
                }
            });

            return entity;
        }
    }
}
