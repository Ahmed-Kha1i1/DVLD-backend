using DVLD.Domain.Common;

namespace DVLD.Domain.Entities
{
    public class LocalApplication : BaseEntity
    {
        public int LicenseClassID { get; set; }
        public LicenseClass? LicenseClassInfo;
    }
}
