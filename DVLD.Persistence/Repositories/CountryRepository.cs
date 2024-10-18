using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.Countries;
using DVLD.Domain.Common.Enums;
using DVLD.Domain.Entities;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;
using Microsoft.Data.SqlClient;

namespace DVLD.Persistence.Repositories
{
    public class CountryRepository : BaseRepository, ICountryRepository
    {
        public CountryRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {
        }

        public async Task<Country?> GetByIdAsync(int id)
        {
            Country? entity = null;

            await _dataSendhandler.Handle("SP_GetCountryById", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue($"@CountryId", id);
                Connection.Open();
                using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        entity = _mapper.Map<Country>(reader);
                        entity.Mode = enMode.Update;
                    }
                }
            });

            return entity;
        }

        public async Task<Country?> GetByNameAsync(string CountryName)
        {
            Country? entity = null;

            await _dataSendhandler.Handle("SP_GetCountryByName", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue($"@CountryName", CountryName);
                Connection.Open();
                using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        entity = _mapper.Map<Country>(reader);
                        entity.Mode = enMode.Update;
                    }
                }
            });

            return entity;
        }

        public async Task<IReadOnlyList<CountryDTO>> ListAsync()
        {
            return await ListAllAsync<CountryDTO>("SP_GetAllCountries");
        }

    }
}
