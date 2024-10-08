using BusinessLayer.Country;
using BusinessLayerCore;
using DataLayerCore.Person;

namespace BusinessLayer
{
    [Serializable]
    public class clsPerson
    {
        enum enMode { AddNew, Update }
        enMode Mode = enMode.AddNew;

        public int? PersonID { get; set; }
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

        public DateTime DateOfBirth { get; set; }
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
        public string? Email { get; set; }
        public int? NationalityCountryID { get; set; }
        public clsCountry? CountryInfo;
        public string? ImageName { get; set; }



        public clsPerson()
        {
            PersonID = null;
            NationalNo = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            DateOfBirth = default;
            Gender = 0;
            Address = "";
            Phone = "";
            Email = "";
            NationalityCountryID = null;
            ImageName = "";

            Mode = enMode.AddNew;
        }

        private clsPerson(PersonDTO Person)
        {
            this.PersonID = Person.PersonID;
            this.NationalNo = Person.NationalNo;
            this.FirstName = Person.FirstName;
            this.SecondName = Person.SecondName;
            this.ThirdName = Person.ThirdName;
            this.LastName = Person.LastName;
            this.DateOfBirth = Person.DateOfBirth;
            this.Gender = Person.Gender;
            this.Address = Person.Address;
            this.Phone = Person.Phone;
            this.Email = Person.Email;
            this.NationalityCountryID = Person.NationalityCountryID;
            this.ImageName = Person.ImageName;
            Mode = enMode.Update;
        }

        public static async Task<clsPerson> CreateAsync(PersonDTO PersonDTO)
        {
            Task<clsCountry?> Country = clsCountry.Find(PersonDTO.NationalityCountryID);

            clsPerson Person = new clsPerson(PersonDTO);


            Person.CountryInfo = await Country;

            return Person;
        }

        public static async Task<clsPerson?> Find(int PersonID)
        {

            var Person = await clsPersonData.GetPerson(PersonID);
            if (Person is not null)
            {
                return await CreateAsync(Person);
            }

            return null;
        }

        public static async Task<clsPerson?> Find(string NationalNo)
        {

            var Person = await clsPersonData.GetPerson(NationalNo);
            if (Person is not null)
            {
                return await CreateAsync(Person);
            }


            return null;
        }
        private async Task<bool> _AddNewPerson()
        {
            var NewPerson = AutoMapperConfig.Mapper.Map<PersonDTO>(this);
            PersonID = await clsPersonData.AddNewPerson(NewPerson);

            return PersonID is not null;
        }

        private async Task<bool> _UpdatePerson()
        {
            var UpdatePerson = AutoMapperConfig.Mapper.Map<PersonDTO>(this);
            return await clsPersonData.UpdatePerson(PersonID ?? -1, UpdatePerson);
        }

        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (await _AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return await _UpdatePerson();


            }

            return false;
        }

        public static async Task<bool> DeletePerson(int PersonID)
        {
            return await clsPersonData.DeletePerson(PersonID);
        }

        public static async Task<List<PersonFullDTO>> GetPeopleDetails()
        {
            return await clsPersonData.GetAllPeople();
        }



        public static async Task<bool> IsNationalNoUnique(string NationalNo, int? Id = null)
        {
            return await clsPersonData.IsNationalNoUnique(NationalNo, Id);
        }

        public static async Task<bool> IsEmailUnique(string Email, int? Id = null)
        {
            return await clsPersonData.IsEmailUnique(Email, Id);
        }

        public static async Task<bool> IsPhoneUnique(string Phone, int? Id = null)
        {
            return await clsPersonData.IsPhoneUnique(Phone, Id);
        }

        public static async Task<bool> IsPersonExists(int PersonID)
        {
            return await clsPersonData.IsPersonExists(PersonID);
        }
    }
}
