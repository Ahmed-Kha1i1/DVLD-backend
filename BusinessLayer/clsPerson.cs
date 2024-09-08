using BusinessLayer.Country;
using BusinessLayerCore;
using DataLayerCore.Person;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        get {
                string ThirdNameWithSpace = ThirdName != "" ? $"{ThirdName} " : "";
                return $"{FirstName} {SecondName} {ThirdNameWithSpace} {LastName}"; 
            }
        }

        public DateTime DateOfBirth { get; set; }
        public enGender Gendor { get; set; }

        public string GendorCaption
        {
            get
            {
                return Gendor == 0 ? "Male" : "Female";
            }
        }

        public string Address { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public int? NationalityCountryID { get; set; }
        public clsCountry? CountryInfo;
        public string? ImagePath { get; set; }


        public clsPerson()
        {
            PersonID = null;
            NationalNo = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            DateOfBirth = default;
            Gendor = 0;
            Address = "";
            Phone = "";
            Email = "";
            NationalityCountryID = null;
            ImagePath = "";

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
            this.Gendor = Person.Gendor;
            this.Address = Person.Address;
            this.Phone = Person.Phone;
            this.Email = Person.Email;
            this.NationalityCountryID = Person.NationalityCountryID;
            this.ImagePath = Person.ImagePath;
            Mode = enMode.Update;
        }

        protected static async Task<clsPerson> CreateAsync(PersonDTO PersonDTO)
        {
            Task<clsCountry?> Country = clsCountry.Find(PersonDTO.NationalityCountryID);

            clsPerson Person = new clsPerson(PersonDTO);


            Person.CountryInfo = await Country;

            return Person;
        }

        public static async Task <clsPerson?> Find(int PersonID)
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
            var NewPerson = AutoMapperConfig.Mapper.Map<PersonForCreateDTO>(this);
            PersonID = await clsPersonData.AddNewPerson(NewPerson);

            return PersonID is not null;
        }

        private async Task<bool> _UpdatePerson()
        {
            var UpdatePerson = AutoMapperConfig.Mapper.Map<PersonForUpdateDTO>(this);
            return await clsPersonData.UpdatePerson(PersonID ??-1, UpdatePerson);
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

        

        public static async Task<bool> IsPersonExists(string NationalNo)
        {
            return await clsPersonData.IsPersonExists(NationalNo);
        }

        public static async Task<bool> IsPersonExists(int PersonID)
        {
            return await clsPersonData.IsPersonExists(PersonID);
        }
    }
}
