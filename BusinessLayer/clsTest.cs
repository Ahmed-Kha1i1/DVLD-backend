using BusinessLayer.Tests.Test_Appointment;
using BusinessLayerCore;
using DataLayerCore.Test;
using DataLayerCore.TestType;
using System.Data;
using static DataLayerCore.Test.clsTestData;

namespace BusinessLayer.Tests.Test
{
    public class clsTest
    {
        enum enMode { AddNew, Update }
        enMode Mode = enMode.AddNew;

        public int? TestID { get; set; }
        public int? TestAppointmentID { get; set; }
        public clsTestAppointment? TestAppointmentInfo { get; set; }
        public bool TestResult { get; set; }
        public string? Notes { get; set; }
        public int? CreatedByUserID { get; set; }

        public clsTest()
        {
            TestID = null;
            TestResult = false;
            Notes = "";
            CreatedByUserID = null;

            Mode = enMode.AddNew;
        }

        private clsTest(TestDTO Test)
        {
            this.TestID = Test.TestID;
            this.TestResult = Test.TestResult;
            this.Notes = Test.Notes;
            this.CreatedByUserID = Test.CreatedByUserID;

            Mode = enMode.Update;
        }

        private static async Task<clsTest> CreateAsync(TestDTO TestDTO)
        {
            Task<clsTestAppointment?> TestAppointment = clsTestAppointment.Find(TestDTO.TestAppointmentID);

            clsTest Test = new clsTest(TestDTO);


            Test.TestAppointmentInfo = await TestAppointment;


            return Test;
        }

        public static async Task<clsTest?> Find(int TestID)
        {


            var Test = await clsTestData.GetTest(TestID);
            if (Test is not null)
            {
                return await CreateAsync(Test);
            }
                

            return null;
        }

        public static async Task<clsTest?> FindLastTestPerPersonAndLicenseClass
            (enTestType TestTypeID, LastTestDTO lastTestDTO)
        {
            var Test = await clsTestData.GetLastTest(TestTypeID,lastTestDTO);
            if (Test is not null)
            {
                return await CreateAsync(Test);
            }


            return null;

        }

        private async Task<bool> _AddNewTest()
        {
            var NewTest = AutoMapperConfig.Mapper.Map<TestForCreateDTO>(this);
            TestID = await clsTestData.AddNewTest(NewTest);

            return TestID is not null;
        }

        private async Task<bool> _UpdateTest()
        {
            var UpdateTest = AutoMapperConfig.Mapper.Map<TestForUpdateDTO>(this);
            return await clsTestData.UpdateTest(TestID ?? -1, UpdateTest);
        }

        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (await _AddNewTest())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return await _UpdateTest();


            }

            return false;
        }

        public static async Task<List<TestDTO>> GetTests()
        {
            return await clsTestData.GetTests();
        }


        public static async Task<int?> GetTestpassCount(int LocalDrivingLicenseApplicationID)
        {
            return await clsTestData.GetTestpassCount(LocalDrivingLicenseApplicationID);
        }

        public static async Task<bool> IsPassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            return await GetTestpassCount(LocalDrivingLicenseApplicationID) == 3;
        }


    }
}
