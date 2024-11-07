using DVLD.Domain.Common;

namespace DVLD.Domain.Entities
{
    public class Driver : BaseEntity
    {
        public int PersonID { set; get; }
        public Person? PersonInfo;
        public int? CreatedByUserID { set; get; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public byte NumberofActiveLicenses { get; set; }
    }
}
