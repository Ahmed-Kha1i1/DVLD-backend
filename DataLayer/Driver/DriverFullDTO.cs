using DataLayerCore.Person;

namespace DataLayerCore.Driver
{
    public class DriverFullDTO
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte NumberofActiveLicenses { get; set; }
        public PersonFullDTO Person { get; set; }
    }
}
