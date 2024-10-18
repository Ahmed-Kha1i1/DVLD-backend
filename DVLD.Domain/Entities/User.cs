using DVLD.Domain.Common;

namespace DVLD.Domain.Entities
{
    public class User : BaseEntity
    {
        public int PersonID { get; set; }
        public Person? PersonInfo;
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
