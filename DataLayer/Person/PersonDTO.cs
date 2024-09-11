namespace DataLayerCore.Person
{
    public class PersonDTO : PersonBaseDTO
    {

        public enGender Gendor { get; set; }
        public int NationalityCountryID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string LastName { get; set; }

    }
}