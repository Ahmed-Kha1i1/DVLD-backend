using BusinessLayer;

namespace ApiLayer.Services
{
    public class UserService
    {
        public async Task<bool> BeUniqueUsername(string Usernmae, int? Id = null)
        {
            return await clsUser.IsUsernameUnique(Usernmae, Id);
        }
    }
}
