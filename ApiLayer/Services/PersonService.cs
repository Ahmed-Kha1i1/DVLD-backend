using BusinessLayer;

namespace ApiLayer.Services
{
    public class PersonService
    {

        public async Task<bool> BeUniqueNationalNo(string nationalNo, int? Id = null)
        {
            return await clsPerson.IsNationalNoUnique(nationalNo, Id);
        }

        public async Task<bool> BeUniquePhone(string phone, int? Id = null)
        {
            return await clsPerson.IsPhoneUnique(phone, Id);
        }

        public async Task<bool> BeUniqueEmail(string email, int? Id = null)
        {
            return await clsPerson.IsEmailUnique(email, Id);
        }
    }
}
