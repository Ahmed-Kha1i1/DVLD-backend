using BusinessLayer.ApplicationsDescendants.Applications;
using BusinessLayer.ApplicationTypes;
using BusinessLayer.License;
using BusinessLayer.LicenseClass;
using BusinessLayer.Tests.Test;
using BusinessLayer.Tests.TestTypes;
using BusinessLayerCore;
using DataLayerCore.Application;
using DataLayerCore.License;
using DataLayerCore.LocalDrivingLicenseApplication;
using DataLayerCore.TestType;
using static DataLayerCore.LocalDrivingLicenseApplication.clsLocalDrivingLicenseApplicationData;
using static DataLayerCore.Test.clsTestData;


namespace BusinessLayer.NewLocalLicesnse
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        enum enMode { AddNew, Update }
        enMode Mode = enMode.AddNew;

        public int? LocalDrivingLicenseApplicationID { get; set; }
        public int? LicenseClassID { get; set; }
        public clsLicenseClass? LicenseClassInfo;

        public string PersonFullName
        {
            get
            {
                if (PersonInfo is not null)
                    return PersonInfo.FullName;
                else
                    return "";
            }

        }

        public clsLocalDrivingLicenseApplication()
        {
            LocalDrivingLicenseApplicationID = null;
            LicenseClassID = null;

            Mode = enMode.AddNew;
        }

        private clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationDTO LocalDrivingLicenseApplication)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            this.LicenseClassID = LocalDrivingLicenseApplication.LicenseClassID;

            this.Mode = enMode.Update;
        }

        protected static async Task<clsLocalDrivingLicenseApplication> CreateAsync(LocalDrivingLicenseApplicationDTO LocalDrivingLicenseApplicationDTO)
        {
            Task<clsLicenseClass?> LicenseClass = clsLicenseClass.Find(LocalDrivingLicenseApplicationDTO.LicenseClassID);
            clsApplication Application = await FindApplication(LocalDrivingLicenseApplicationDTO.ApplicationID);
            
            var ApplicationDTO = AutoMapperConfig.Mapper.Map<ApplicationDTO>(Application);
            await CreateAsync(ApplicationDTO);
            clsLocalDrivingLicenseApplication LocalApplication = new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationDTO);


            LocalApplication.LicenseClassInfo = await LicenseClass;

            return LocalApplication;

        }
        public static async Task<clsLocalDrivingLicenseApplication?> FindByID(int LocalDrivingLicenseApplicationID)
        {
            var local = await clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationByID(LocalDrivingLicenseApplicationID);
            if (local is not null)
            {
                return await CreateAsync(local);
            }
            else
            {
                return null;
            }
        }

        public static async Task<clsLocalDrivingLicenseApplication?> FindByApplicationID(int ApplicationID)
        {
            var local = await clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationByApplicationID(ApplicationID);
            if (local is not null)
            {
                return await CreateAsync(local);
            }
            else
            {
                return null;

            }
        }

        private async Task<bool> _AddNewLocalDrivingLicenseApplication()
        {
            var NewLocalApplication = AutoMapperConfig.Mapper.Map<LocalDrivingLicenseApplicationForCreateDTO>(this);
            LocalDrivingLicenseApplicationID = await clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(NewLocalApplication);

            return LocalDrivingLicenseApplicationID is not null;
        }

        private async Task<bool> _UpdateLocalDrivingLicenseApplication()
        {
            var UpdateLocalApplciation = AutoMapperConfig.Mapper.Map<LocalDrivingLicenseApplicationForUpdateDTO>(this);
            return await clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID ?? -1, UpdateLocalApplciation);
        }

        public async Task<bool> Save()
        {
            base.Mode = (clsApplication.enMode)Mode;

            if(! await base.Save())
                return false;

            switch (Mode)
            {
                case enMode.AddNew:

                    if (await _AddNewLocalDrivingLicenseApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return await _UpdateLocalDrivingLicenseApplication();


            }

            return false;
        }

        public async Task<bool> Delete()
        {
            if (await clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID ?? -1))
            {
                return await base.Delete();
            }
            else
            {
                return false;
            }
        }

        public static async Task<List<LocalDrivigFullData>> GetLocalDrivingLicenseApplications()
        {
            return await clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplications();
        }

        public static async Task<bool> DoesPassTestType(int LocalDrivingLicenseApplicationID, enTestType TestTypeID)
        {
            return await clsLocalDrivingLicenseApplicationData.DoesPassTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public async Task<bool> DoesPassTestType(enTestType TestTypeID)
        {
            return await clsLocalDrivingLicenseApplicationData.DoesPassTestType(LocalDrivingLicenseApplicationID ?? -1, (int)TestTypeID);
        }

        public static async Task<bool> DoesAttendTestType(int LocalDrivingLicenseApplicationID, enTestType TestTypeID)
        {
            return await clsLocalDrivingLicenseApplicationData.DoesAttendTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public async Task<bool> DoesAttendTestType( enTestType TestTypeID)
        {
            return await clsLocalDrivingLicenseApplicationData.DoesAttendTestType(LocalDrivingLicenseApplicationID ?? -1, (int)TestTypeID);
        }

        public static async Task<byte> TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, enTestType TestTypeID)
        {
            return await clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public async Task<byte> TotalTrialsPerTest( enTestType TestTypeID)
        {
            return await clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(LocalDrivingLicenseApplicationID ?? -1, (int)TestTypeID);
        }

        public static async Task<bool> IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, enTestType TestTypeID)
        {
            return await clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID,(int)TestTypeID);
        }

        public async Task<bool> IsThereAnActiveScheduledTest(enTestType TestTypeID)
        {
            return await clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID ?? -1, (int)TestTypeID);
        }

        public async Task<bool> DoesPassPreviousTest(enTestType CurrentTestType)
        {

            switch (CurrentTestType)
            {
                case enTestType.VisionTest:
                    return true;

                case enTestType.WrittenTest:
                    return await this.DoesPassTestType(enTestType.VisionTest);


                case enTestType.StreetTest:
                    return await this.DoesPassTestType(enTestType.WrittenTest);

                default:
                    return false;
            }
        }

        public async Task<int?> GetTestpassCount()
        {
            return await clsTest.GetTestpassCount(LocalDrivingLicenseApplicationID ?? -1);
        }

        public async Task<bool> IsLicenseIssued()
        {
            return (await GetActiveLicenseID() is not null);
        }

        public async Task<int?> GetActiveLicenseID()
        {
            return await clsLicense.GetActiveLicenseIDByPersonID(ApplicantPersonID ?? -1, LicenseClassID ?? -1);
        }

        public static async Task<clsTest?> GetLastTestPerTestType(enTestType TestTypeID, LastTestDTO lastTestDTO)
        {
            return await clsTest.FindLastTestPerPersonAndLicenseClass(TestTypeID, lastTestDTO);
        }

        public async Task<clsTest?> GetLastTestPerTestType(enTestType TestTypeID)
        {
            var LastTestDTO = new LastTestDTO()
            {
                PersonID = ApplicantPersonID ?? -1,
                LicenseClassID = LicenseClassID ?? -1
            };
            return await clsTest.FindLastTestPerPersonAndLicenseClass(TestTypeID, LastTestDTO);
        }

        public static async Task<bool> passedAllTest(int LocalDrivingLicenseApplicationID)
        {
            return await clsTest.IsPassedAllTests(LocalDrivingLicenseApplicationID);
        }

        public async Task<bool> passedAllTest()
        {
            return await clsTest.IsPassedAllTests(LocalDrivingLicenseApplicationID ?? -1);
        }
        public class IssueLocalLicenseDTO
        {
            public string Notes { get; set; }
            public int UserId { get; set; }
        }
        public async Task<int?> IssueLocalLicense(IssueLocalLicenseDTO issueLocalLicenseDTO)
        {
            int? DriverID = null;
            clsDriver? Driver = await clsDriver.FindByPersonID(ApplicantPersonID ?? -1);

            if(Driver is null)
            {
                Driver = new clsDriver();
                Driver.PersonID = ApplicantPersonID;
                Driver.CreatedByUserID = issueLocalLicenseDTO.UserId;
                Driver.CreatedDate = DateTime.Now;
                if (await Driver.Save())
                {
                    DriverID = Driver.DriverID;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                DriverID = Driver.DriverID;
            }

            clsLicense License = new clsLicense();
            License.ApplicationID = ApplicationID;
            License.DriverID = DriverID;
            License.LicenseClassID = LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(LicenseClassInfo.DefaultValidityLength);
            License.Notes = issueLocalLicenseDTO.Notes;
            License.PaidFees = LicenseClassInfo.ClassFees;
            License.IsActive = true;
            License.IssueReason = enIssueReason.FirstTime;
            License.CreatedByUserID = issueLocalLicenseDTO.UserId;

            if (await License.Save())
            {
                await this.SetCompleted();
                return License.LicenseID;
            }

            return null;
        }
    }
}
