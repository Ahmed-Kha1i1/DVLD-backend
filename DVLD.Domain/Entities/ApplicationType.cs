using DVLD.Domain.Common;

namespace DVLD.Domain.Entities
{
    public class ApplicationType : BaseEntity
    {
        public string ApplicationTypeTitle { get; set; }
        public float ApplicationFees { get; set; }
    }
}
