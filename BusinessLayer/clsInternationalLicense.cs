using BusinessLayer.ApplicationsDescendants.Applications;
using BusinessLayerCore;
using DataLayerCore.Application;
using DataLayerCore.ApplicationType;
using DataLayerCore.InternationalLicense;


namespace BusinessLayer.InternationalLicense
{
    public class clsInternationalLicense : clsApplication
    {
        enum enMode { AddNew, Update }
        enMode Mode = enMode.AddNew;

        public int? InternationalLicenseID { get; set; }
        public int? DriverID { get; set; }
        public clsDriver? DriverInfo;
        public int? IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }



        public clsInternationalLicense()
        {
            InternationalLicenseID = null;
            DriverID = null;
            IssuedUsingLocalLicenseID = null;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            IsActive = true;

            Mode = enMode.AddNew;
        }

        private clsInternationalLicense(ApplicationDTO Application, InternationalLicenseDTO InternationalLicense)
        {
            base.ApplicationID = Application.ApplicationID;
            base.ApplicantPersonID = Application.ApplicantPersonID;
            base.ApplicationDate = Application.ApplicationDate;
            base.ApplicationTypeID = enApplicationType.NewInternationalLicense;
            base.ApplicationStatusID = Application.ApplicationStatus;
            base.LastStatusDate = Application.LastStatusDate;
            base.PaidFees = Application.PaidFees;
            base.CreatedByUserID = Application.CreatedByUserID;

            this.InternationalLicenseID = InternationalLicense.InternationalLicenseID;
            this.DriverID = InternationalLicense.DriverID;
            this.IssuedUsingLocalLicenseID = InternationalLicense.IssuedUsingLocalLicenseID;
            this.IssueDate = InternationalLicense.IssueDate;
            this.ExpirationDate = InternationalLicense.ExpirationDate;
            this.IsActive = InternationalLicense.IsActive;

            Mode = enMode.Update;
        }

        private static async Task<clsInternationalLicense> CreateAsync(InternationalLicenseDTO InternationalLicenseDTO)
        {
            Task<clsDriver?> Driver = clsDriver.FindByDriverID(InternationalLicenseDTO.DriverID);
            clsApplication Application = await FindApplication(InternationalLicenseDTO.ApplicationID);
            var ApplicationDTO = AutoMapperConfig.Mapper.Map<ApplicationDTO>(Application);
            clsInternationalLicense InternationalLicense = new clsInternationalLicense(ApplicationDTO, InternationalLicenseDTO);


            InternationalLicense.DriverInfo = await Driver;

            return InternationalLicense;
        }

        public static async Task<clsInternationalLicense?> FindInternationalLicense(int InternationalLicenseID)
        {
            var internationalicense = await clsInternationalLicenseData.GetInternationalLicense(InternationalLicenseID);
            if (internationalicense is not null )
            {
                return await CreateAsync(internationalicense);
            }
            else
            {
                return null;
            }
        }
               
        
        private async Task<bool> _AddNewInternationalLicense()
        {
            var NewInternationalLicense = AutoMapperConfig.Mapper.Map<InternationalLicenseForCreateDTO>(this);
            InternationalLicenseID = await clsInternationalLicenseData.AddNewInternationalLicense(NewInternationalLicense);

            return InternationalLicenseID is not null;
        }

        private async Task<bool> _UpdateInternationalLicense()
        {
            var InternationalLicense = AutoMapperConfig.Mapper.Map<InternationalLicenseForUpdateDTO>(this);
            return await clsInternationalLicenseData.UpdateInternationalLicense(InternationalLicenseID ?? -1, InternationalLicense);
        }

        public async Task<bool> Save()
        {
            base.Mode = (clsApplication.enMode)Mode;
            if (!await base.Save())
                return false;

            switch (Mode)
            {
                case enMode.AddNew:

                    if (await _AddNewInternationalLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return await _UpdateInternationalLicense();


            }

            return false;
        }


        public static async Task<List<InternationalLicenseDTO>> GetInternationalLicenses()
        {
            return await clsInternationalLicenseData.GetInternationalLicenses();
        }
        public static async Task<List<InternationalLicenseDTO>> GetInternationalLicenses(int DriverID)
        {
            return await  clsInternationalLicenseData.GetInternationalLicenses(DriverID);
        }

        public static async Task<int?> GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return await  clsInternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }
    }
}
