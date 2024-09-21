using BusinessLayer.ApplicationsDescendants.Applications;
using BusinessLayer.ApplicationTypes;
using BusinessLayer.LicenseClass;
using System.Xml.Serialization;
using System;
using static System.Net.Mime.MediaTypeNames;
using DataAccessLayer;
using DataLayerCore.Application;
using DataLayerCore.ApplicationType;
using DataLayerCore.License;
using BusinessLayerCore;
using DataLayerCore.DetainedLicense;
using DataLayerCore.Driver;


namespace BusinessLayer.License
{

    
    public class clsLicense
    {
        
        public enum enMode 
        {
            AddNew,
            Update 
        }
        public enMode Mode = enMode.AddNew;

       
        public int? LicenseID { get; set; }
        public int? ApplicationID { get; set; }
        public int? DriverID { get; set; }
        public clsDriver? DriverInfo;
        public int? LicenseClassID { get; set; }
        public clsLicenseClass? LicenseClassInfo;
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Notes { get; set; }
        public float PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason { get; set; }
        public int? CreatedByUserID { get; set; }
        public async Task<bool> IsDetained()
        {
            return await clsDetainedLicense.IsLicenseDetained(LicenseID ?? -1);
        }

        public clsDetainedLicense? DetainedInfo;
        
        public string IssueReasonTest
        {
            get
            {
                return GetIssueReasonText(IssueReason);
            }
        }
        public clsLicense()
        {
            LicenseID = null;
            ApplicationID = null;
            DriverID = null;
            LicenseClassID = null;
            IssueDate = DateTime.Now;
            ExpirationDate = default;
            Notes = "";
            PaidFees = 0;
            IsActive = true;
            IssueReason = enIssueReason.FirstTime;
            CreatedByUserID = null;

            Mode = enMode.AddNew;
        }

        private clsLicense(LicenseInfoDTO License)
        {
            this.LicenseID = License.LicenseID;
            this.ApplicationID = License.ApplicationID;
            this.DriverID = License.DriverID;
            this.LicenseClassID = License.LicenseClass;
            this.IssueDate = License.IssueDate;
            this.ExpirationDate = License.ExpirationDate;
            this.Notes = License.Notes;
            this.PaidFees = License.PaidFees;
            this.IsActive = License.IsActive;
            this.IssueReason = License.IssueReason;
            this.CreatedByUserID = License.CreatedByUserID;

            Mode = enMode.Update;
        }

        private static async Task<clsLicense> CreateAsync(LicenseInfoDTO LicenseDTO)
        {
            Task<clsDriver?> Driver = clsDriver.FindByDriverID(LicenseDTO.DriverID);
            Task<clsLicenseClass?> LicenseClass = clsLicenseClass.Find(LicenseDTO.LicenseClass);
            Task<clsDetainedLicense?> Detained = clsDetainedLicense.Find(LicenseDTO.LicenseID);

            clsLicense License = new clsLicense(LicenseDTO);


            License.DriverInfo= await Driver;
            License.LicenseClassInfo= await LicenseClass;
            License.DetainedInfo= await Detained;

            return License;

        }
        public static async Task<clsLicense?> FindByLicenseID(int LicenseID)
        {

            var License = await clsLicenseData.GetLicenseInfoByLicenseID(LicenseID);
            if (License is not null)

                return await CreateAsync(License);
            else
                return null;

        }

        public static async Task<clsLicense?> FindByApplicationID(int ApplicationID)
        {

            var License = await clsLicenseData.GetLicenseInfoByApplicationID(ApplicationID);
            if (License is not null)

                return await CreateAsync(License);
            else
                return null;
        }

        public static string GetIssueReasonText(enIssueReason IssueReason)
        {

            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.ReplacementDamaged:
                    return "Replacement for Damaged";
                case enIssueReason.ReplacementLost:
                    return "Replacement for Lost";
                default:
                    return "First Time";
            }
        }

        private async Task<bool> _AddNewLicens()
        {
            var NewLicense =  AutoMapperConfig.Mapper.Map<LicenseInfoForCreateDTO>(this);
            LicenseID = await clsLicenseData.AddNewLicense(NewLicense);

            return LicenseID is not null;
        }

        private async Task<bool> _UpdateLicens()
        {
            var UpdateLicense =  AutoMapperConfig.Mapper.Map<LicenseInfoForUpdateDTO>(this);
            return await  clsLicenseData.UpdateLicense(LicenseID ?? -1, UpdateLicense);
        }

        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (await _AddNewLicens())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return await _UpdateLicens();


            }

            return false;
        }

        public static async Task<List<LicenseInfoDTO>> GetLicenses()
        {
            return await clsLicenseData.GetLicenses();
        }
        public static async Task<List<DriverLicenseDTO>> GetDriverLicenses(int DriverID)
        {
            return await clsLicenseData.GetDriverLicenses(DriverID);
        }

        public static async Task<int?> GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            return await clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);   
        }

        public static async Task<bool> IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return (await GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) is not null);
        }

        public static async Task<bool> DeactivateLicense(int LicenseID)
        {
            return await clsLicenseData.DeactivateLicense(LicenseID); 
        }

        public async Task<bool> DeactivateLicense()
        {
            return await clsLicenseData.DeactivateLicense(LicenseID ?? -1);
        }

        public bool IsLicenseExpired()
        {
            return this.ExpirationDate < DateTime.Now;
        }

        public async Task<clsLicense?> RenewLicense(RenewLicenseDTO replaceLicenseDTO)
        {
            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = this.DriverInfo?.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = enApplicationType.RenewDrivingLicense;
            Application.ApplicationStatusID = enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = (await clsApplicationType.Find(enApplicationType.RenewDrivingLicense))?.ApplicationFees ?? 0;
            Application.CreatedByUserID = replaceLicenseDTO.CreatedUserID;

            if (! await Application.Save())
            {
                return null;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;

            int? DefaultValidityLength = this.LicenseClassInfo?.DefaultValidityLength;

            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength ?? 0);
            NewLicense.Notes = replaceLicenseDTO.Notes;
            NewLicense.PaidFees = this.LicenseClassInfo?.ClassFees ?? 0;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = enIssueReason.Renew;
            NewLicense.CreatedByUserID = replaceLicenseDTO.CreatedUserID;


            if (! await NewLicense.Save())
            {
                return null;
            }

            await DeactivateLicense();

            return NewLicense;
        }

        public async Task<clsLicense?> ReplaceFor(ReplaceLicenseDTO replaceLicenseDTO)
        {

            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = this.DriverInfo?.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = replaceLicenseDTO.IssueReson == enIssueReason.ReplacementDamaged ?
                enApplicationType.ReplaceDamagedDrivingLicense :
                enApplicationType.ReplaceLostDrivingLicense;
            Application.ApplicationStatusID = enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = (await clsApplicationType.Find(Application.ApplicationTypeID))?.ApplicationFees ?? 0;
            Application.CreatedByUserID = replaceLicenseDTO.CreatedUserID;


            if (! await Application.Save())
            {
                return null;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = 0;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = replaceLicenseDTO.IssueReson;
            NewLicense.CreatedByUserID = replaceLicenseDTO.CreatedUserID;


            if (! await NewLicense.Save())
            {
                return null;
            }

            await DeactivateLicense();

            return NewLicense;
        }

        public async Task<int?> Detain(DetainLicenseDTO detainedLicenseDTO)
        {
            clsDetainedLicense detainedLicense = new clsDetainedLicense();
            detainedLicense.LicenseID = this.LicenseID;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = Convert.ToSingle(detainedLicenseDTO.FineFees);
            detainedLicense.CreatedByUserID = detainedLicenseDTO.CreatedByUserId;

            if (! await detainedLicense.Save())
            {

                return null;
            }

            return detainedLicense.DetainID;
        }

        
        // return ApplicationID is success else null
        public async Task<int?> ReleaseDetainedLicense(int ReleasedByUserID)
        {
            int? ApplicationID = null;
            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = this.DriverInfo?.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = enApplicationType.ReleaseDetainedDrivingLicsense;
            Application.ApplicationStatusID = enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = (await clsApplicationType.Find(enApplicationType.ReleaseDetainedDrivingLicsense))?.ApplicationFees ?? 0;
            Application.CreatedByUserID = ReleasedByUserID;

            if (! await Application.Save())
            {
                return null;
            }

            ApplicationID = Application.ApplicationID;
            var ReleaseDetainedLicenseDTO = new ReleaseDetainedLicenseDTO() 
            {
                ReleaseApplicationID = Application.ApplicationID ?? -1,
                DetainID = DetainedInfo.DetainID ?? -1,
                ReleasedByUserID = ReleasedByUserID
            };

            bool detailed = DetainedInfo is not null ? await clsDetainedLicense.ReleaseDetainedicense(ReleaseDetainedLicenseDTO) : false;

            if (detailed)
            {
                return ApplicationID;
            }
            else
            {
                return null;
            }
        }
    }
}
