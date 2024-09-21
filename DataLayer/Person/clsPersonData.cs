using DataLayerCore;
using DataLayerCore.Datahandler;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Net;
using System.Numerics;

namespace DataLayerCore.Person
{
    public enum enGender { Male = 0, Female = 1 }
    public class clsPersonData
    {
        public static async Task<PersonDTO?> GetPerson(int PersonID)
        {
            PersonDTO? personDTO = null;
            await DataSendhandler.handle("SP_FindPersondById", async (Connection, Command) =>
            {

                Command.Parameters.AddWithValue("@PersonID", PersonID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        personDTO = Reader.MapTo<PersonDTO>();
                    }
                }


            });

            return personDTO;

        }

        public static async Task<PersonDTO?> GetPerson(string NationalNo)
        {
            PersonDTO? personDTO = null;
            await DataSendhandler.handle("SP_FindPersondByNationalNo", async (Connection, Command) =>
            {

                Command.Parameters.AddWithValue("@NationalNo", NationalNo);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        personDTO = Reader.MapTo<PersonDTO>();
                    }
                }
            });

            return personDTO;
        }

        public static async Task<int?> AddNewPerson(PersonForCreateDTO Person)
        {
            int? personID = null;
            await DataSendhandler.handle("SP_AddNewPerson", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@NationalNo", Person.NationalNo);
                Command.Parameters.AddWithValue("@FirstName", Person.FirstName);
                Command.Parameters.AddWithValue("@SecondName", Person.SecondName);
                if (string.IsNullOrWhiteSpace(Person.ThirdName))
                {
                    Command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@ThirdName", Person.ThirdName);
                }
                Command.Parameters.AddWithValue("@LastName", Person.LastName);
                Command.Parameters.AddWithValue("@DateOfBirth", Person.DateOfBirth);
                Command.Parameters.AddWithValue("@Gendor", Person.Gendor);
                Command.Parameters.AddWithValue("@Address", Person.Address);
                Command.Parameters.AddWithValue("@Phone", Person.Phone);
                if (string.IsNullOrWhiteSpace(Person.Email))
                {
                    Command.Parameters.AddWithValue("@Email", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@Email", Person.Email);
                }
                Command.Parameters.AddWithValue("@NationalityCountryID", Person.NationalityCountryID);
                if (string.IsNullOrWhiteSpace(Person.ImageName))
                {
                    Command.Parameters.AddWithValue("@ImageName", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@ImageName", Person.ImageName);
                }
                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    personID = insertedID;
                }
            });

            return personID;

        }

        public static async Task<bool> UpdatePerson(int PersonId, PersonForUpdateDTO Person)
        {

            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdatePerson", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@PersonID", PersonId);
                Command.Parameters.AddWithValue("@NationalNo", Person.NationalNo);
                Command.Parameters.AddWithValue("@NationalNo", Person.NationalNo);
                Command.Parameters.AddWithValue("@FirstName", Person.FirstName);
                Command.Parameters.AddWithValue("@SecondName", Person.SecondName);
                if (string.IsNullOrWhiteSpace(Person.ThirdName))
                {
                    Command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@ThirdName", Person.ThirdName);
                }
                Command.Parameters.AddWithValue("@LastName", Person.LastName);
                Command.Parameters.AddWithValue("@DateOfBirth", Person.DateOfBirth);
                Command.Parameters.AddWithValue("@Gendor", Person.Gendor);
                Command.Parameters.AddWithValue("@Address", Person.Address);
                Command.Parameters.AddWithValue("@Phone", Person.Phone);
                if (string.IsNullOrWhiteSpace(Person.Email))
                {
                    Command.Parameters.AddWithValue("@Email", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@Email", Person.Email);
                }
                Command.Parameters.AddWithValue("@NationalityCountryID", Person.NationalityCountryID);
                if (string.IsNullOrWhiteSpace(Person.ImageName))
                {
                    Command.Parameters.AddWithValue("@ImageName", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@ImageName", Person.ImageName);
                }

                RowAffected = await Command.ExecuteNonQueryAsync();

            });

            return RowAffected > 0;

        }

        public static async Task<bool> DeletePerson(int PersonID)
        {
            int RowAffected = 0;
            await DataSendhandler.handle("SP_DeletePersonById", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@PersonID", PersonID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;

        }

        public static async Task<List<PersonFullDTO>> GetAllPeople()
        {

            var list = new List<PersonFullDTO>();
            await DataSendhandler.handle("SP_GetAllPeople", async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<PersonFullDTO>());
                    }
                }
            });


            return list;

        }

        public static async Task<bool> IsPersonExists(string NationalNo)
        {
            bool IsFound = false;
            await DataSendhandler.handle("SP_IsPersonExistsByNationalNo", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@NationalNo", NationalNo);
                object? Result = await Command.ExecuteScalarAsync();

                IsFound = Result is not null;
            });

            return IsFound;
        }

        public static async Task<bool> IsPersonExists(int PersonID)
        {
            bool IsFound = false;
            await DataSendhandler.handle("SP_IsPersonExists", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@PersonID", PersonID);
                object? Result = await Command.ExecuteScalarAsync();

                IsFound = Result is not null;
            });

            return IsFound;
        }
    }
}