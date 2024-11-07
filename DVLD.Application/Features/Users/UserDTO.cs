using DVLD.Application.Features.People.Common.Models;

namespace DVLD.Application.Features.Users
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public PersonDTO Person { get; set; }
    }
}
