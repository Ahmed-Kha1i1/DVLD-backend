using BusinessLayerCore;
using DataLayerCore.TestType;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Tests.TestTypes
{
    public class clsTestType
    {
        enum enMode { AddNew, Update }
        enMode Mode = enMode.AddNew;

        

        public enTestType TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public float TestTypeFees { get; set; }

        public clsTestType()
        {
            TestTypeID = enTestType.None;
            TestTypeTitle = "";
            TestTypeDescription = "";
            TestTypeFees = 0;

            Mode = enMode.AddNew;
        }

        private clsTestType(TestTypeDTO TestType)
        {
            this.TestTypeID = TestType.TestTypeID;
            this.TestTypeTitle = TestType.TestTypeTitle;
            this.TestTypeDescription = TestType.TestTypeDescription;
            this.TestTypeFees = TestType.TestTypeFees;

            Mode = enMode.Update;
        }

        public static async Task<clsTestType?> Find(enTestType TestTypeID)
        {
            var TestType = await clsTestTypeData.GetTestType(TestTypeID);

            if (TestType is not null)
            {
                return new clsTestType(TestType);
            }

            return null;
        }


        public static async Task<List<TestTypeDTO>> GetTestTypes()
        {
            return await clsTestTypeData.GetTestTypes();
        }

    }
}
