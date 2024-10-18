using DVLD.Domain.Common.Enums;

namespace DVLD.Domain.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public enMode Mode { get; set; } = enMode.AddNew;
    }
}
