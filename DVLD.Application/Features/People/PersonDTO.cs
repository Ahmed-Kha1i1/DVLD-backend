namespace DVLD.Application.Features.People
{
    public class PersonDTO
    {
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                string ThirdNameWithSpace = ThirdName != "" ? $"{ThirdName} " : "";
                return $"{FirstName} {SecondName} {ThirdNameWithSpace} {LastName}";
            }
        }

        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string? ImageName { get; set; }
        public string Gender { get; set; }
        public string CountryName { get; set; }
    }
}
