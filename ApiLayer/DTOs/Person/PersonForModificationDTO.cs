using DataLayerCore.Person;

namespace ApiLayer.DTOs.Person
{
    public abstract class PersonForModificationDTO
    {
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public enGender Gender { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int NationalityCountryID { get; set; }

    }
}
