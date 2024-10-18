using DVLD.Domain.Common;
using DVLD.Domain.Common.Enums;

namespace DVLD.Domain.Entities
{
    public class Application : BaseEntity
    {
        public int ApplicantPersonID { get; set; }
        public Person? PersonInfo;
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public enApplicationType ApplicationTypeID { get; set; }
        public ApplicationType? ApplicationType;
        public enApplicationStatus ApplicationStatusID { get; set; } = enApplicationStatus.New;
        public DateTime LastStatusDate { get; set; }
        public float PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public User? UserInfo;


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
    }
}
