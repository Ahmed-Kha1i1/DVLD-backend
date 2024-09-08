using Microsoft.Data.SqlClient;
using DataLayerCore.Datahandler;

namespace DataLayerCore.Country
{
    public class clsCountryData
    {
        public static async Task<CountryDTO?> GetCountry(int CountryID)
        {
            CountryDTO? countryDTO = null;
            await DataSendhandler.handle("SP_GetCountryById", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@CountryId", CountryID);
                SqlDataReader Reader = await Command.ExecuteReaderAsync();

                if (Reader.Read())
                {
                    countryDTO = Reader.MapTo<CountryDTO>();
                }
            });


            return countryDTO;
        }

        public static async Task<CountryDTO?> GetCountry(string CountryName)
        {
            CountryDTO? countryDTO = null;
            await DataSendhandler.handle("SP_GetCountryByName", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@CountryName", CountryName);
                SqlDataReader Reader = await Command.ExecuteReaderAsync();

                if (Reader.Read())
                {

                    countryDTO = Reader.MapTo<CountryDTO>();
                }
            });

            return countryDTO;

        }

        public static async Task<List<CountryDTO>> GetAllCountries()
        {

            var list = new List<CountryDTO>();

            await DataSendhandler.handle("SP_GetAllCountries", async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<CountryDTO>());
                    }
                }
            });
            return list;
        }

    }

}
