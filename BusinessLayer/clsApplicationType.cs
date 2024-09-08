using DataLayerCore.ApplicationType;

namespace BusinessLayer.ApplicationTypes
{
    public class clsApplicationType
    {
        enum enMode { AddNew, Update }
        enMode Mode = enMode.AddNew;

       
        public enApplicationType ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public float ApplicationFees { get; set; }
        public clsApplicationType()
        {
            ApplicationTypeID = enApplicationType.None;
            ApplicationTypeTitle = "";
            ApplicationFees = 0;

            Mode = enMode.AddNew;
        }

        private clsApplicationType(ApplicationTypeDTO ApplicationType)
        {
            this.ApplicationTypeID = ApplicationType.ApplicationTypeID;
            this.ApplicationTypeTitle = ApplicationType.ApplicationTypeTitle;
            this.ApplicationFees = ApplicationType.ApplicationFees;

            Mode = enMode.Update;
        }

        public static async Task<clsApplicationType?> Find(enApplicationType ApplicationTypeID)
        {
            ApplicationTypeDTO? ApplicationType = await clsApplicationTypeData.GetApplicationType(ApplicationTypeID);
            if (ApplicationType is not null)
            {
                return new clsApplicationType(ApplicationType);

            }

            return null;
        }

        public static async Task<List<ApplicationTypeDTO>> GetApplicationTypes()
        {
            return await clsApplicationTypeData.GetApplicationTypes();
        }

       
    }
}
