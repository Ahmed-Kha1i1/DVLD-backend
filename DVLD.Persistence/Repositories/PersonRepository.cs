using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.People.Common.Models;
using DVLD.Application.Features.People.Queries.GetPeopleQuery;
using DVLD.Domain.Common.Enums;
using DVLD.Domain.Entities;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DVLD.Persistence.Repositories
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {
        }

        public async Task<Person?> GetByIdAsync(int PersonId)
        {
            return await GetEntityAsync<Person>("SP_FindPersondById", PersonId, "PersonID");
        }

        public async Task<Person?> GetAsync(string NationalNo)
        {
            Person? Person = null;

            await _dataSendhandler.Handle("SP_FindPersondByNationalNo", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue($"@NationalNo", NationalNo);
                Connection.Open();
                using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        Person = _mapper.Map<Person>(reader);
                        Person.Mode = enMode.Update;
                    }
                }
            });

            return Person;
        }

        protected override async Task<int?> AddAsync(Person person)
        {
            int? personID = null;
            await _dataSendhandler.Handle("SP_AddNewPerson", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@NationalNo", person.NationalNo);
                Command.Parameters.AddWithValue("@FirstName", person.FirstName);
                Command.Parameters.AddWithValue("@SecondName", person.SecondName);
                if (string.IsNullOrWhiteSpace(person.ThirdName))
                {
                    Command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@ThirdName", person.ThirdName);
                }
                Command.Parameters.AddWithValue("@LastName", person.LastName);
                Command.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth);
                Command.Parameters.AddWithValue("@Gender", person.Gender);
                Command.Parameters.AddWithValue("@Address", person.Address);
                Command.Parameters.AddWithValue("@Phone", person.Phone);
                if (string.IsNullOrWhiteSpace(person.Email))
                {
                    Command.Parameters.AddWithValue("@Email", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@Email", person.Email);
                }
                Command.Parameters.AddWithValue("@NationalityCountryID", person.NationalityCountryID);
                if (string.IsNullOrWhiteSpace(person.ImageName))
                {
                    Command.Parameters.AddWithValue("@ImageName", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@ImageName", person.ImageName);
                }
                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    personID = insertedID;
                }
            });

            return personID;
        }

        protected override async Task<bool> UpdateAsync(Person person)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_UpdatePerson", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@PersonID", person.Id);
                Command.Parameters.AddWithValue("@NationalNo", person.NationalNo);
                Command.Parameters.AddWithValue("@FirstName", person.FirstName);
                Command.Parameters.AddWithValue("@SecondName", person.SecondName);
                if (string.IsNullOrWhiteSpace(person.ThirdName))
                {
                    Command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@ThirdName", person.ThirdName);
                }
                Command.Parameters.AddWithValue("@LastName", person.LastName);
                Command.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth);
                Command.Parameters.AddWithValue("@Gender", person.Gender);
                Command.Parameters.AddWithValue("@Address", person.Address);
                Command.Parameters.AddWithValue("@Phone", person.Phone);
                if (string.IsNullOrWhiteSpace(person.Email))
                {
                    Command.Parameters.AddWithValue("@Email", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@Email", person.Email);
                }
                Command.Parameters.AddWithValue("@NationalityCountryID", person.NationalityCountryID);
                if (string.IsNullOrWhiteSpace(person.ImageName))
                {
                    Command.Parameters.AddWithValue("@ImageName", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@ImageName", person.ImageName);
                }

                RowAffected = await Command.ExecuteNonQueryAsync();

            });

            return RowAffected > 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync("SP_DeletePersonById", id, "PersonID");
        }

        public async Task<IReadOnlyList<PersonOverviewDTO>> ListOverviewAsync()
        {
            return await ListAllAsync<PersonOverviewDTO>("SP_GetAllPeople");
        }

        public async Task<(IReadOnlyList<PersonOverviewDTO> items, int totalCount)> ListOverviewAsync(GetPeopleQuery request)
        {
            List<PersonOverviewDTO> Items = new();

            int TotalCount = 0;
            await _dataSendhandler.Handle("SP_GetAllPeople", async (Connection, Command) =>
            {
                var TotalCountParameter = new SqlParameter("@TotalCount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                Command.Parameters.AddWithValue("@SearchQuery", request.SearchQuery);
                Command.Parameters.AddWithValue("@Id", request.Id);
                Command.Parameters.AddWithValue("@NationalNumber", request.NationalNumber);
                Command.Parameters.AddWithValue("@Gender", request.Gender);
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
                        Items.Add(_mapper.Map<PersonOverviewDTO>(reader));
                    }
                }
                TotalCount = (int?)TotalCountParameter.Value ?? 0;
            });

            return (Items, TotalCount);
        }

        public async Task<bool> IsNationalNoUnique(string NationalNo, int? PersonId)
        {
            return await IsUnique("SP_IsNationalNoUnique", NationalNo, "NationalNo", PersonId);
        }

        public async Task<bool> IsEmailUnique(string Email, int? PersonId)
        {
            return await IsUnique("SP_IsEmailUnique", Email, "Email", PersonId);
        }

        public async Task<bool> IsPhoneUnique(string Phone, int? PersonId)
        {
            return await IsUnique("SP_IsPhoneUnique", Phone, "Phone", PersonId);
        }

        public async Task<bool> IsPersonExists(int PersonId)
        {
            return await CheckConditionAsync("SP_IsPersonExists", PersonId, "PersonID");
        }

        public async Task<int?> GetDriverId(int PersonId)
        {
            int? DriverId = null;
            await _dataSendhandler.Handle("SP_GetDriverId", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue($"@PersonId", PersonId);
                object? Result = await Command.ExecuteScalarAsync();

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int id))
                {
                    DriverId = id;

                }
            });

            return DriverId;
        }
    }
}
