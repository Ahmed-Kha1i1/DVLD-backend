using DVLD.Domain.Common.Enums;

namespace DVLD.Application.Features.TestAppointment.Common.Requests.ListPerTestTypeRequest
{
    public class LocalAppTestTypeRequest
    {
        public int LocalApplicationId { get; set; }
        public enTestType TestTypeId { get; set; }
    }
}
