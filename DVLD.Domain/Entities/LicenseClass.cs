using DVLD.Domain.Common;

namespace DVLD.Domain.Entities
{
    public class LicenseClass : BaseEntity
    {
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public float ClassFees { get; set; }
    }
}
