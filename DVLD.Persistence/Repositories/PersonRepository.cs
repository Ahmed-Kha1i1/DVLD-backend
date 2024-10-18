using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.People;
using DVLD.Domain.Common.Enums;
using DVLD.Domain.Entities;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;
using Microsoft.Data.SqlClient;

namespace DVLD.Persistence.Repositories
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        protected readonly IMapper _mapper;
        public PersonRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {
            _mapper = mapper;
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            return await GetItemAsync<Person>("SP_FindPersondById", id, "PersonID");
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

        public async Task<IReadOnlyList<PersonOverviewDTO>> ListPeopleOverviewAsync()
        {
            return await ListAllAsync<PersonOverviewDTO>("SP_GetAllPeople");
        }

        public async Task<bool> IsNationalNoUnique(string NationalNo, int? Id)
        {
            return await IsUnique("SP_IsNationalNoUnique", NationalNo, "NationalNo", Id);
        }

        public async Task<bool> IsEmailUnique(string Email, int? Id)
        {
            return await IsUnique("SP_IsEmailUnique", Email, "Email", Id);
        }

        public async Task<bool> IsPhoneUnique(string Phone, int? Id)
        {
            return await IsUnique("SP_IsPhoneUnique", Phone, "Phone", Id);
        }
        public async Task<bool> IsPersonExists(int Id)
        {
            return await CheckConditionAsync("SP_IsPersonExists", Id, "PersonID");
        }
    }
}
