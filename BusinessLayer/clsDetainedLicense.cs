using BusinessLayerCore;
using DataLayerCore.DetainedLicense;
using DataLayerCore.Driver;



namespace BusinessLayer
{
    [Serializable]
    public class clsDetainedLicense
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int? DetainID { set; get; }
        public int? LicenseID { set; get; }
        public DateTime DetainDate { set; get; }

        public float FineFees { set; get; }
        public int? CreatedByUserID;
        public clsUser? CreatedByUserInfo { set; get; }
        public bool IsReleased { set; get; }
        public DateTime? ReleaseDate { set; get; }
        public int? ReleasedByUserID { set; get; }
        public clsUser? ReleasedByUserInfo;
        public int? ReleaseApplicationID { set; get; }


        public clsDetainedLicense()

        {
            this.DetainID = null;
            this.LicenseID = null;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = null;
            this.IsReleased = false;
            this.ReleaseDate = null;
            this.ReleasedByUserID = null;
            this.ReleaseApplicationID = null;



            Mode = enMode.AddNew;

        }

        public clsDetainedLicense(DetainedLicenseDTO DetainedLicense)

        {
            this.DetainID = DetainedLicense.DetainID;
            this.LicenseID = DetainedLicense.LicenseID;
            this.DetainDate = DetainedLicense.DetainDate;
            this.FineFees = DetainedLicense.FineFees;
            this.CreatedByUserID = DetainedLicense.CreatedByUserID;
            this.IsReleased = DetainedLicense.IsReleased;
            this.ReleaseDate = DetainedLicense.ReleaseDate;
            this.ReleasedByUserID = DetainedLicense.ReleasedByUserID;
            this.ReleaseApplicationID = DetainedLicense.ReleaseApplicationID;

            Mode = enMode.Update;
        }

        private static async Task<clsDetainedLicense> CreateAsync(DetainedLicenseDTO DetainedLicenseDTO)
        {
            Task<clsUser?> CreatedByUser = clsUser.FindUser(DetainedLicenseDTO.CreatedByUserID);
            Task<clsUser?>? ReleasedByUser = DetainedLicenseDTO.ReleasedByUserID is not null ?  clsUser.FindUser(DetainedLicenseDTO.ReleasedByUserID ?? -1) : null;

            clsDetainedLicense DetainedLicense = new clsDetainedLicense(DetainedLicenseDTO);


            DetainedLicense.CreatedByUserInfo = await CreatedByUser;
            DetainedLicense.ReleasedByUserInfo = ReleasedByUser is null ?  null : await ReleasedByUser;


            return DetainedLicense;
        }

        public static async Task<clsDetainedLicense?> FindByLicenseID(int LicenseID)
        {
            DetainedLicenseDTO? DetainedLicense = await clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseID(LicenseID);
            if (DetainedLicense is not null)

                return await CreateAsync(DetainedLicense);
            else

                return null;
        }

        private async Task<bool> _AddNewDetainedLicense()
        {
            var NewDetainedLicense = AutoMapperConfig.Mapper.Map<DetainedLicenseForCreateDTO>(this);
            this.DetainID = await clsDetainedLicenseData.AddNewDetainedLicense(NewDetainedLicense);

            return (this.DetainID is not null);
        }

        private async Task<bool> _UpdateDetainedLicense()
        {
            var UpdateDetainedLicense = AutoMapperConfig.Mapper.Map<DetainedLicenseForUpdateDTO>(this);
            return await clsDetainedLicenseData.UpdateDetainedLicense(DetainID ?? -1,UpdateDetainedLicense);
        }

        public static async Task<clsDetainedLicense?> Find(int DetainID)
        {

            DetainedLicenseDTO? DetainedLicense = await clsDetainedLicenseData.GetDetainedLicenseInfoByID(DetainID);
            if (DetainedLicense is not null)

                return new clsDetainedLicense(DetainedLicense);
            else
                return null;

        }

        public static async Task<List<DetainedLicenseFullDTO>> GetAllDetainedLicenses()
        {
            return await clsDetainedLicenseData.GetAllDetainedLicenses();

        }

        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewDetainedLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return await _UpdateDetainedLicense();

            }

            return false;
        }

        public static async Task<bool> IsLicenseDetained(int LicenseID)
        {
            return await clsDetainedLicenseData.IsLicenseDetained(LicenseID);
        }

        public static async Task<bool> ReleaseDetainedicense(ReleaseDetainedLicenseDTO ReleaseDetainedLicense)
        {
            return await clsDetainedLicenseData.ReleaseDetainedLicense(ReleaseDetainedLicense);
        }
    }
}
