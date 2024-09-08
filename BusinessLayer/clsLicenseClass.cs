using BusinessLayerCore;
using DataLayerCore.LicenseClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.LicenseClass
{
    public class clsLicenseClass
    {
        enum enMode { AddNew, Update }
        enMode Mode = enMode.AddNew;

        public int? LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public float ClassFees { get; set; }

        public clsLicenseClass()
        {
            LicenseClassID = null;
            ClassName = "";
            ClassDescription = "";
            MinimumAllowedAge = 18;
            DefaultValidityLength = 10;
            ClassFees = 0;

            Mode = enMode.AddNew;
        }

        private clsLicenseClass(LicenseClassDTO licenseClass)
        {
            this.LicenseClassID = licenseClass.LicenseClassID;
            this.ClassName = licenseClass.ClassName;
            this.ClassDescription = licenseClass.ClassDescription;
            this.MinimumAllowedAge = licenseClass.MinimumAllowedAge;
            this.DefaultValidityLength = licenseClass.DefaultValidityLength;
            this.ClassFees = licenseClass.ClassFees;

            Mode = enMode.Update;
        }

        public static async Task<clsLicenseClass?> Find(int LicenseClassID)
        {

            var LicenseClass = await clsLicenseClassData.GetLicenseClassInfoByID(LicenseClassID);

            if (LicenseClass is not null)
                return new clsLicenseClass(LicenseClass);

            return null;
        }

        public static async Task<clsLicenseClass?> Find(string ClassName)
        {

            var LicenseClass = await clsLicenseClassData.GetLicenseClassInfoByClassName(ClassName);

            if (LicenseClass is not null)
                return new clsLicenseClass(LicenseClass);

            return null;

        }

     

        public static async Task<List<LicenseClassDTO>> GetAllLicenseClasses()
        {
            return await clsLicenseClassData.GetAllLicenseClasses();
        }


    }
}
