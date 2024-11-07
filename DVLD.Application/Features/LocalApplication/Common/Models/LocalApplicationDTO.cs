using DVLD.Application.Features.Application.Common.Model;

namespace DVLD.Application.Features.LocalApplication.Common.Models
{
    public class LocalApplicationDTO
    {
        public int LocalApplicationId { get; set; }

        public int ApplicationId { get; set; }
        public string ClassName { get; set; }
        public byte PassedTestCount { get; set; }
        public ApplicationOverviewDTO? basicApplication { get; set; }
    }
}
