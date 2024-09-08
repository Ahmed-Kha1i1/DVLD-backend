using BusinessLayerCore;
using DataLayerCore.User;
using System;
using System.Data;
using System.IO;
using static DataLayerCore.User.clsUserData;

namespace BusinessLayer
{
    public class clsUser
    {
        enum enMode { AddNew, Update }
        enMode Mode = enMode.AddNew;

        public int? UserID { get; set; }
        public int? PersonID { get; set; }
        public clsPerson? PersonInfo;
        public string UserName { get; set; }
        public bool IsActive { get; set; }

        public clsUser()
        {
            UserID = null;
            PersonID = null;
            UserName = "";
            IsActive = true;

            Mode = enMode.AddNew;
        }

        private clsUser(UserDTO User)
        {
            this.UserID = User.UserID;
            this.PersonID = User.PersonID;
            this.UserName = User.UserName;
            this.IsActive = User.IsActive;

            Mode = enMode.Update;
        }

        protected static async Task<clsUser> CreateAsync(UserDTO UserDTO)
        {
            Task<clsPerson?> Person = clsPerson.Find(UserDTO.PersonID);

            clsUser User = new clsUser(UserDTO);


            User.PersonInfo = await Person;

            return User;
        }

        public static async Task<clsUser?> FindUser(int UserID)
        {

            var User = await clsUserData.GetUser(UserID);
            if (User is not null)
            {
                return await CreateAsync(User);
            }
                

            return null;
        }

        public static async Task<clsUser?> FindUserByPersonID(int PersonID)
        {

            var User = await clsUserData.GetUserByPersonID(PersonID);
            if (User is not null)
            {
                return await CreateAsync(User);
            }


            return null;
        }

        public static async Task<clsUser?> FindUserInfoByCredentials(string UserName, string Password)
        {



            var User = await clsUserData.GetUserInfoByCredentials(UserName, Password);
            if (User is not null)
            {
                return await CreateAsync(User);
            }


            return null;

        }


        private async Task<bool> _AddNewUser()
        {
            var NewUser = AutoMapperConfig.Mapper.Map<UserForCreateDTO>(this);
            UserID = await clsUserData.AddNewUser(NewUser);

            return UserID is not null;
        }

        private async Task<bool> _UpdateUser()
        {
            var updateUser = AutoMapperConfig.Mapper.Map<UserForUpdateDTO>(this);
            return await clsUserData.UpdateUser(UserID ?? -1, updateUser);
        }

        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (await _AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return await _UpdateUser();


            }

            return false;
        }

        public static async Task<bool> DeleteUser(int UserID)
        {
            return  await clsUserData.DeleteUser(UserID);
        }

        public static async Task<List<UserFullDTO>> GetAllUsers()
        {
            return await clsUserData.GetAllUsers();
        }
        public static async Task<bool> IsUserExistByUserID(int UserID)
        {
            return await clsUserData.IsUserExistByUserID(UserID);
        }

        public static async Task<bool> IsUserExistByPersonID(int PersonID)
        {
            return await clsUserData.IsUserExistByPersonID(PersonID);
        }

        public static async Task<bool> IsUserExistByUserName(string UserName)
        {
            return await clsUserData.IsUserExistByUserName(UserName);
        }

        public static async Task<bool> IsUserActive(int UserID)
        {
            return await clsUserData.IsUserActive(UserID);
        }

        public static async Task<bool> UpdatePassword(int UserID, string NewPassword)
        {
            return await clsUserData.UpdatePassword(UserID, NewPassword);
        }
    }
}
