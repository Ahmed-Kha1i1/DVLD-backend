using AutoMapper;
using BusinessLayer.ApplicationTypes;
using BusinessLayerCore;
using DataLayerCore.Application;
using DataLayerCore.ApplicationType;
using DataLayerCore.Driver;
namespace BusinessLayer.ApplicationsDescendants.Applications
{
    public class clsApplication
    {
        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;


        public int? ApplicationID { get; set; }
        public int? ApplicantPersonID { get; set; }
        public clsPerson? PersonInfo;
        public DateTime ApplicationDate { get; set; }
        public enApplicationType ApplicationTypeID { get; set; }
        public clsApplicationType? ApplicationType;
        public enApplicationStatus ApplicationStatusID { get; set; }
        public DateTime  LastStatusDate { get; set; }
        public float PaidFees { get; set; }
        public int? CreatedByUserID { get; set; }
        public clsUser? UserInfo;


        public string StatusText
        {
            get
            {

                switch (ApplicationStatusID)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }

        }

        public clsApplication()
        {
            ApplicationID = null;
            ApplicantPersonID = null;
            ApplicationDate = DateTime.Now;
            ApplicationTypeID = enApplicationType.None;
            ApplicationStatusID = enApplicationStatus.New;
            LastStatusDate = DateTime.Now;
            PaidFees = 0;
            CreatedByUserID = null;

            
            Mode = enMode.AddNew;
        }

        private clsApplication(ApplicationDTO application)
        {
            this.ApplicationID = application.ApplicationID;
            this.ApplicantPersonID = application.ApplicantPersonID;
            this.ApplicationDate = application.ApplicationDate;
            this.ApplicationTypeID = application.ApplicationTypeID;
            this.ApplicationStatusID = application.ApplicationStatus;
            this.LastStatusDate = application.LastStatusDate;
            this.PaidFees = application.PaidFees;
            this.CreatedByUserID = application.CreatedByUserID;
            
            Mode = enMode.Update;
        }
        
        protected static async Task<clsApplication> CreateAsync(ApplicationDTO applicationDTO)
        {
            Task<clsPerson?> Person = clsPerson.Find(applicationDTO.ApplicantPersonID);
            Task<clsApplicationType?> ApplicationType = clsApplicationType.Find(applicationDTO.ApplicationTypeID);
            Task<clsUser?> User = clsUser.FindUser(applicationDTO.CreatedByUserID);

            clsApplication Application = new clsApplication(applicationDTO);

            
            Application.PersonInfo = await Person;
            Application.ApplicationType = await ApplicationType;
            Application.UserInfo = await User;

            return Application;
        }

        public static  async Task<clsApplication?> FindApplication(int ApplicationID)
        {
            ApplicationDTO? applcaition = await clsApplicationData.GetApplication(ApplicationID);
            if (applcaition is not null)
            {
                return await CreateAsync(applcaition);
            }

            return null;
        }
        private async Task<bool> _AddNewApplication()
        {
            var NewApplication = AutoMapperConfig.Mapper.Map<ApplicationForCreateDTO>(this);
            ApplicationID = await clsApplicationData.AddNewApplication(NewApplication);

            return ApplicationID is not null;
        }

        private async Task<bool> _UpdateApplication()
        {
            var UpdateApplciation = AutoMapperConfig.Mapper.Map<ApplicationForUpdateDTO>(this);
            return await clsApplicationData.UpdateApplication(ApplicationID ?? -1, UpdateApplciation);
        }

        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (await _AddNewApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return await _UpdateApplication();


            }

            return false;
        }

        public static async Task<bool> DeleteApplication(int ApplicationID)
        {
            return await  clsApplicationData.DeleteApplication(ApplicationID);
        }

        public static async Task<List<ApplicationDTO>> GetApplications()
        {
            return await  clsApplicationData.GetApplications();
        }

        public static async Task<bool> IsApplicationExist(int ApplicationID)
        {
            return await  clsApplicationData.IsApplicationExist(ApplicationID);
        }

        public static async Task<bool> UpdateStatus(int ApplicationID, enApplicationStatus ApplicationStatus)
        {
            return await  clsApplicationData.UpdateStatus(ApplicationID, ApplicationStatus);
        }

        public async Task<bool> Cancel()
        {
            return await clsApplicationData.UpdateStatus(this.ApplicationID ?? -1, enApplicationStatus.Cancelled);
        }
        public async Task<bool> SetCompleted()
        {
            return await clsApplicationData.UpdateStatus(this.ApplicationID ?? -1, enApplicationStatus.Completed);
        }

        public async Task<bool> Delete()
        {
            return await clsApplicationData.DeleteApplication(this.ApplicationID ?? -1);
        }

        public static async Task<bool> DoesPersonHasActiveApplication(int ApplicantPersonID, enApplicationType ApplicationTypeID)
        {
            return await clsApplicationData.DoesPersonHasActiveApplication(ApplicantPersonID, ApplicationTypeID);
        }

        public async Task<bool> DoesPersonHasActiveApplication(enApplicationType ApplicationTypeID)
        {
            return await clsApplicationData.DoesPersonHasActiveApplication(this.ApplicantPersonID ?? -1, ApplicationTypeID);
        }

        public static async Task<int?> GetActiveApplicationID(int ApplicantPersonID, enApplicationType ApplicationTypeID)
        {
            return await clsApplicationData.GetActiveApplicationID(ApplicantPersonID, ApplicationTypeID);
        }

        public async Task<int?> GetActiveApplicationID(enApplicationType ApplicationTypeID)
        {
            return await clsApplicationData.GetActiveApplicationID(this.ApplicantPersonID ?? -1, ApplicationTypeID);
        }

        public static async Task<int?> GetActiveApplicationIDForLicenseClass(int ApplicantPersonID, enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            return await clsApplicationData.GetActiveApplicationIDForLicenseClass(ApplicantPersonID, ApplicationTypeID, LicenseClassID);
        }

        public static async Task<bool> IsApplicationCancelled(int ApplicationID)
        {
            return await clsApplicationData.IsApplicationCancelled(ApplicationID);
        }

        public async Task<bool> IsCancelled()
        {
            return await IsApplicationCancelled(ApplicationID ?? -1);
        }
    }
}
