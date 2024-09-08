using DataLayerCore.Country;

namespace BusinessLayer.Country
{
    [Serializable]
    public class clsCountry
    {
        public int? CountryID { get; set; }
        public string CountryName { get; set; }

        private clsCountry(CountryDTO Country)
        {
            CountryID = Country.CountryID;
            CountryName = Country.CountryName;
        }

        public clsCountry() 
        {
            CountryID = null;
            CountryName = "";
        }


        public static async Task<clsCountry?> Find(int CountryID)
        {
            CountryDTO? Country = await clsCountryData.GetCountry(CountryID);
            if (Country is not null)
            {
                return new clsCountry(Country); 
            }

            return null;
        }

        public static async Task<clsCountry?> Find(string CountryName)
        {
            CountryDTO? Country = await clsCountryData.GetCountry(CountryName);
            if (Country is not null)
            {
                return new clsCountry(Country);
            }


            return null;
        }

        public static async Task<List<CountryDTO>>  GetAllCountries()
        {
            return await clsCountryData.GetAllCountries();
        }

    }
}
