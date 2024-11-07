namespace DVLD.Application.Features.People.Common.Models
{
    public class PersonOverviewDTO
    {
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? ImageName { get; set; }
        public string Gender { get; set; }
        public string CountryName { get; set; }
    }
}
