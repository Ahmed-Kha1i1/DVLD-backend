using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.DetainedLicense.Common.Models;
using DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicensesQuery;
using DVLD.Domain.Entities;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DVLD.Persistence.Repositories
{
    public class DetainedLicenseRepository : GenericRepository<DetainedLicense>, IDetainedLicenseRepository
    {
        public DetainedLicenseRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {
        }

        public override async Task<bool> DeleteAsync(int detainedLicenseId)
        {
            return await DeleteAsync("SP_DeleteDetainedLicenseById", detainedLicenseId, "DetainId");
        }

        public async Task<DetainedLicense?> GetByIdAsync(int detainedLicenseId)
        {
            return await GetEntityAsync<DetainedLicense>("SP_FindDetainedLicensById", detainedLicenseId, "DetainId");
        }

        public async Task<DetainedLicense?> GetByLicenseIdAsync(int LicenseID)
        {
            return await GetEntityAsync<DetainedLicense?>("SP_GetDetainedLicenseInfoByLicenseID", LicenseID, "LicenseID");
        }

        public async Task<bool> IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;
            await _dataSendhandler.Handle("SP_IsLicenseDetained", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@LicenseID", LicenseID);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null)
                {
                    IsDetained = Convert.ToBoolean(result);
                }
            });
            return IsDetained;
        }

        public async Task<IReadOnlyList<DetainedLicenseOverviewDTO>> ListOverviewAsync()
        {
            return await ListAllAsync<DetainedLicenseOverviewDTO>("SP_GetAllDetainedLicenses");
        }

        public async Task<(IReadOnlyList<DetainedLicenseOverviewDTO> items, int totalCount)> ListOverviewAsync(GetDetainedLicensesQuery request)
        {
            List<DetainedLicenseOverviewDTO> Items = new();

            int TotalCount = 0;
            await _dataSendhandler.Handle("SP_GetAllDetainedLicenses", async (Connection, Command) =>
            {
                var TotalCountParameter = new SqlParameter("@TotalCount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                Command.Parameters.AddWithValue("@SearchQuery", request.SearchQuery);
                Command.Parameters.AddWithValue("@Id", request.Id);
                Command.Parameters.AddWithValue("@IsReleased", request.IsReleased);
                Command.Parameters.AddWithValue("@NationalNumber", request.NationalNumber);
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
                        Items.Add(_mapper.Map<DetainedLicenseOverviewDTO>(reader));
                    }
                }
                TotalCount = (int?)TotalCountParameter.Value ?? 0;
            });

            return (Items, TotalCount);
        }

        public async Task<bool> ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int rowsAffected = 0;
            await _dataSendhandler.Handle("SP_ReleaseDetainedLicense", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@DetainId", DetainID);
                Command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
                Command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
                Command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
                rowsAffected = await Command.ExecuteNonQueryAsync();

            });

            return rowsAffected > 0;
        }

        protected override async Task<int?> AddAsync(DetainedLicense entity)
        {
            int? DetainID = null;
            await _dataSendhandler.Handle("SP_AddNewDetainedLicens", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@LicenseID", entity.LicenseID);
                Command.Parameters.AddWithValue("@DetainDate", entity.DetainDate);
                Command.Parameters.AddWithValue("@FineFees", entity.FineFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);

                object? Result = await Command.ExecuteScalarAsync();

                if (Result is not null && int.TryParse(Result.ToString(), out int ID))
                {
                    DetainID = ID;
                }
            });

            return DetainID;
        }

        protected override async Task<bool> UpdateAsync(DetainedLicense entity)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_UpdateDetainedLicens", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@DetainedLicenseID", entity.Id);
                Command.Parameters.AddWithValue("@LicenseID", entity.LicenseID);
                Command.Parameters.AddWithValue("@DetainDate", entity.DetainDate);
                Command.Parameters.AddWithValue("@FineFees", entity.FineFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }
    }
}
