using DVLD.Domain.Common.Enums;
using System.Text.Json.Serialization;

namespace DVLD.Application.Features.Application.Common.Model
{
    public class ApplicationOverviewDTO
    {
        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ApplicationType { get; set; }
        [JsonIgnore]
        public enApplicationStatus ApplicationStatusId { get; set; }
        public string ApplicationStatus
        {
            get
            {
                return ApplicationStatusId.ToString();
            }
        }
        public DateTime LastStatusDate { get; set; }
        public float PaidFees { get; set; }
        public string Username { get; set; }
        public string ApplicantFullName { get; set; }


    }
}
