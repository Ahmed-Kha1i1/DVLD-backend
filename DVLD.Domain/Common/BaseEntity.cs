using DVLD.Domain.Common.Enums;
using System.Text.Json.Serialization;

namespace DVLD.Domain.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        [JsonIgnore]
        public enMode Mode { get; set; } = enMode.AddNew;
    }
}
