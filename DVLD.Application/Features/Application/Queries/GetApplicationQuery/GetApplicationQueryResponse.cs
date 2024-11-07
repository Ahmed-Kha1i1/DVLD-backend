using DVLD.Domain.Common.Enums;

namespace DVLD.Application.Features.Application.Queries.GetApplicationQuery
{
    public class GetApplicationQueryResponse
    {
        public int ApplicationId { get; set; }
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public enApplicationType ApplicationTypeID { get; set; }
        public enApplicationStatus ApplicationStatusID { get; set; }
        public float PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
    }
}
