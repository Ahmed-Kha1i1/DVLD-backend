namespace DataLayerCore.LicenseClass
{
    public abstract class LicenseClassForModificationDTO
    {
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public float ClassFees { get; set; }

        
    }
}
