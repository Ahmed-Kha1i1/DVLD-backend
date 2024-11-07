using DVLD.Domain.Common;
using DVLD.Domain.Common.Enums;

namespace DVLD.Domain.Entities
{
    public class Person : BaseEntity
    {
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
        public enGender Gender { get; set; }

        public string GenderCaption
        {
            get
            {
                return Gender == 0 ? "Male" : "Female";
            }
        }

        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public Country? CountryInfo;
        public string? ImageName { get; set; }

    }
}
