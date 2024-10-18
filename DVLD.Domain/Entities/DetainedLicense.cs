using DVLD.Domain.Common;

namespace DVLD.Domain.Entities
{
    public class DetainedLicense : BaseEntity
    {
        public int LicenseID { set; get; }
        public DateTime DetainDate { set; get; } = DateTime.Now;
        public float FineFees { set; get; }
        public int CreatedByUserID;
        public User? CreatedByUserInfo { set; get; }
        public bool IsReleased { set; get; }
        public DateTime? ReleaseDate { set; get; }
        public int? ReleasedByUserID { set; get; }
        public User? ReleasedByUserInfo;
        public int? ReleaseApplicationID { set; get; }
    }
}
