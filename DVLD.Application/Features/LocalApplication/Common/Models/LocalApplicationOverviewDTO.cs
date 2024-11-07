namespace DVLD.Application.Features.LocalApplication.Common.Models
{
    public class LocalApplicationOverviewDTO
    {
        public int LocalApplicationId { get; set; }
        public string ClassName { get; set; }
        public string NationalNo { get; set; }
        public string FullName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public byte PassedTestCount { get; set; }
        public string Status { get; set; }
        public int PersonId { get; set; }
    }

}
