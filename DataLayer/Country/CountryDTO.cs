using System.ComponentModel.DataAnnotations;

namespace DataLayerCore.Country
{
    public class CountryDTO
    {

        public int CountryID { get; set; }
        [MinLength(10)]
        public string CountryName { get; set; }
       
    }

}
