using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.Driver.Common.Model;
using DVLD.Application.Features.Driver.Queries.GetDriversQuery;
using DVLD.Domain.Entities;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DVLD.Persistence.Repositories
{
    public class DriverRepository : GenericRepository<Driver>, IDriverRepository
    {
        public DriverRepository(DataSendhandler dataSendhandler, AutoMapper.IMapper mapper) : base(dataSendhandler, mapper)
        {
        }

        public override async Task<bool> DeleteAsync(int DriverId)
        {
            return await DeleteAsync("SP_DeleteDriverById", DriverId, "DriverID");
        }

        public async Task<Driver?> GetByIdAsync(int DriverId)
        {
            return await GetEntityAsync<Driver>("SP_FindDriverById", DriverId, "DriverID");
        }

        public async Task<DriverDTO?> GetBypersonId(int personId)
        {
            return await GetAsync<DriverDTO>("SP_FindDriverByPersonID", personId, "PersonID");
        }

        public async Task<DriverDTO?> GetFullAsync(int driverId)
        {
            return await GetAsync<DriverDTO>("SP_FindDriverFull", driverId, "DriverID");
        }

        public async Task<Person?> GetPerson(int driverId)
        {
            return await GetEntityAsync<Person?>("SP_FindPersondByDriverId", driverId, "DriverId");
        }

        public async Task<IReadOnlyList<DriverInternationalLicenseDTO>> ListDriverInternationalLicensesAsync(int driverId)
        {
            List<DriverInternationalLicenseDTO> list = new();

            await _dataSendhandler.Handle("SP_GetAllInternationalLicensesByDriverId", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@DriverID", driverId);
                Connection.Open();
                using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        list.Add(_mapper.Map<DriverInternationalLicenseDTO>(reader));
                    }

                }
            });

            return list;
        }

        public async Task<IReadOnlyList<DriverLicenseDTO>> ListDriverLicensesAsync(int driverId)
        {
            List<DriverLicenseDTO> list = new();

            await _dataSendhandler.Handle("SP_FindDriverLicenses", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@DriverID", driverId);
                Connection.Open();
                using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        list.Add(_mapper.Map<DriverLicenseDTO>(reader));
                    }

                }
            });

            return list;
        }

        public async Task<IReadOnlyList<DriverOverviewDTO>> ListOverviewAsync()

        {
            return await ListAllAsync<DriverOverviewDTO>("SP_GetAllDrivers");
        }

        public async Task<(IReadOnlyList<DriverOverviewDTO> items, int totalCount)> ListOverviewAsync(GetDriversQuery request)
        {
            List<DriverOverviewDTO> items = new();
            int totalCount = 0;

            await _dataSendhandler.Handle("SP_GetAllDrivers", async (connection, command) =>
            {
                var totalCountParam = new SqlParameter("@TotalCount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.AddWithValue("@SearchQuery", request.SearchQuery);
                command.Parameters.AddWithValue("@Id", request.Id);
                command.Parameters.AddWithValue("@PersonID", request.PersonId);
                command.Parameters.AddWithValue("@NationalNumber", request.NationalNumber);
                command.Parameters.AddWithValue("@OrderBy", request.OrderBy);
                command.Parameters.AddWithValue("@OrderDirection", request.OrderDirection);
                command.Parameters.AddWithValue("@PageNumber", request.PageNumber);
                command.Parameters.AddWithValue("@PageSize", request.PageSize);
                command.Parameters.Add(totalCountParam);

                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        items.Add(_mapper.Map<DriverOverviewDTO>(reader));
                    }
                }
                totalCount = (int?)totalCountParam.Value ?? 0;
            });

            return (items, totalCount);
        }

        protected override async Task<int?> AddAsync(Driver entity)
        {
            int? DriverID = null;
            await _dataSendhandler.Handle("SP_AddNewDriver", async (Connection, Command) =>
            {

                Connection.Open();
                Command.Parameters.AddWithValue("@PersonID", entity.PersonID);
                Command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);
                Command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    DriverID = insertedID;
                }
            });

            return DriverID;
        }

        protected override async Task<bool> UpdateAsync(Driver entity)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_UpdateDriver", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@DriverID", entity.Id);
                Command.Parameters.AddWithValue("@PersonID", entity.PersonID);
                Command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);

                RowAffected = await Command.ExecuteNonQueryAsync();

            });

            return RowAffected > 0;
        }
    }
}
