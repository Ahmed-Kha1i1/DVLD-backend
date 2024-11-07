using DVLD.Application.Features.People.Common.Models;

namespace DVLD.Application.Features.Driver.Common.Model
{
    public class DriverDTO
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte NumberofActiveLicenses { get; set; }
        public PersonDTO Person { get; set; }
    }
}
