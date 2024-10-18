using DVLD.Domain.Common;

namespace DVLD.Domain.Entities
{
    public class TestType : BaseEntity
    {
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public float TestTypeFees { get; set; }
    }
}
