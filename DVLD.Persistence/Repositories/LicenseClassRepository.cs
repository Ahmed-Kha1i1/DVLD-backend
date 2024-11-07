using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Domain.Entities;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DVLD.Persistence.Repositories
{
    public class LicenseClassRepository : BaseRepository, ILicenseClassRepository
    {
        public LicenseClassRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {
        }

        public async Task<LicenseClass?> GetByIdAsync(int LIcenseClassId)
        {
            return await GetEntityAsync<LicenseClass>("SP_FindLicenseClassById", LIcenseClassId, "LicenseClassID");
        }

        public async Task<bool> IsPersonAgeValidForLicenseAsync(int personId, int licenseClassId)
        {
            bool IsValid = false;
            await _dataSendhandler.Handle("CheckPersonAgeForLicense", async (Connection, Command) =>
            {
                Connection.Open();
                var outputParam = new SqlParameter("@IsValid", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                Command.Parameters.AddWithValue("@PersonId", personId);
                Command.Parameters.AddWithValue("@LicenseClassId", licenseClassId);
                Command.Parameters.Add(outputParam);

                await Command.ExecuteNonQueryAsync();
                IsValid = (bool)outputParam.Value;
            });

            return IsValid;

        }

        public async Task<IReadOnlyList<LicenseClass>> ListAllAsync()
        {
            return await ListAllAsync<LicenseClass>("SP_GetAllLicenseClasses");
        }
    }
}
