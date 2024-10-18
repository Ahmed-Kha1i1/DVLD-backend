using Microsoft.AspNetCore.Http;

namespace DVLD.Application.Features.People.Commands.ModificationPerson
{
    public class ModificationPersonCommand
    {
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int NationalityCountryID { get; set; }
    }
}
