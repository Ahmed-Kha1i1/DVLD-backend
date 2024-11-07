namespace DVLD.Application.Features.LocalApplication.Common.Models
{
    public class LocalApplicationPrefDTO
    {
        public int LocalApplicationId { get; set; }
        public int ApplicationId { get; set; }
        public int LicenseClassId { get; set; }
        public int PersonId { get; set; }
        public float PaidFees { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Username { get; set; }
    }
}
