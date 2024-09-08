using DataLayerCore.ApplicationType;

namespace DataLayerCore.Application
{
    public abstract class ApplicationForModificationDTO
    {
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public enApplicationType ApplicationTypeID { get; set; }
        public enApplicationStatus ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public float PaidFees { get; set; }
        public int CreatedByUserID { get; set; }

        //public ApplicationForModificationDTO(int applicantPersonID, DateTime applicationDate, enApplicationType applicationTypeID, enApplicationStatus applicationStatus, DateTime lastStatusDate, float paidFees, int createdByUserID)
        //{
        //    ApplicantPersonID = applicantPersonID;
        //    ApplicationDate = applicationDate;
        //    ApplicationTypeID = applicationTypeID;
        //    ApplicationStatus = applicationStatus;
        //    LastStatusDate = lastStatusDate;
        //    PaidFees = paidFees;
        //    CreatedByUserID = createdByUserID;
        //}
    }
}
