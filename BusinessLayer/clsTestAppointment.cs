using BusinessLayer.ApplicationsDescendants.Applications;
using BusinessLayer.Tests.TestTypes;
using BusinessLayerCore;
using DataLayerCore.TestAppointment;
using DataLayerCore.TestType;

namespace BusinessLayer.Tests.Test_Appointment
{
    public class clsTestAppointment
    {
        enum enMode { AddNew, Update }
        enMode Mode = enMode.AddNew;

        public int? TestAppointmentID { get; set; }
        public enTestType TestTypeID { get; set; }
        public int? LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public float PaidFees { get; set; }
        public int? CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int? RetakeTestApplicationID { get; set; }
        public clsApplication? RetakeTestApplication { get; set; }

        public  async Task<int?> TestID()
        {
                return await GetTestID();
        }

        public clsTestAppointment()
        {
            TestAppointmentID = null;
            TestTypeID = enTestType.None;
            LocalDrivingLicenseApplicationID = null;
            AppointmentDate = default;
            PaidFees = 0;
            CreatedByUserID = null;
            IsLocked = false;
            RetakeTestApplicationID = null;

            Mode = enMode.AddNew;
        }

        private clsTestAppointment(TestAppointmentDTO TestAppointment)
        {
            this.TestAppointmentID = TestAppointment.TestAppointmentID;
            this.TestTypeID = TestAppointment.TestTypeID;
            this.LocalDrivingLicenseApplicationID = TestAppointment.LocalDrivingLicenseApplicationID;
            this.AppointmentDate = TestAppointment.AppointmentDate;
            this.PaidFees = TestAppointment.PaidFees;
            this.CreatedByUserID = TestAppointment.CreatedByUserID;
            this.IsLocked = TestAppointment.IsLocked;
            this.RetakeTestApplicationID = TestAppointment.RetakeTestApplicationID;

            Mode = enMode.Update;
        }

        protected static async Task<clsTestAppointment> CreateAsync(TestAppointmentDTO TestAppointmentDTO)
        {
            Task<clsApplication?>? Application = TestAppointmentDTO.RetakeTestApplicationID is not null ? clsApplication.FindApplication(TestAppointmentDTO.RetakeTestApplicationID ?? -1) : null;

            clsTestAppointment TestAppointment = new clsTestAppointment(TestAppointmentDTO);


            TestAppointment.RetakeTestApplication = Application is null ? null : await Application;

            return TestAppointment;
        }
        public static async Task<clsTestAppointment?> Find(int TestAppointmentID)
        {


            var TestAppointment = await clsTestAppointmentData.FindTestAppointment(TestAppointmentID);
            if (TestAppointment is not null)
            {
                return await CreateAsync(TestAppointment);
            }

            return null;
        }

        public static async Task<clsTestAppointment?> FindLastTestAppointment(int LocalDrivingLicenseApplicationID, enTestType TestTypeID)
        {

            var TestAppointment = await clsTestAppointmentData.GetLastTestAppointment(LocalDrivingLicenseApplicationID, TestTypeID);
            if (TestAppointment is not null)
            {
                return await CreateAsync(TestAppointment);
            }

            return null;
        }

        private async Task<bool> _AddNewTestAppointment()
        {
            var NewTestAppointment = AutoMapperConfig.Mapper.Map<TestAppointmentForCreateDTO>(this);
            TestAppointmentID = await clsTestAppointmentData.AddNewTestAppointment(NewTestAppointment);

            return TestAppointmentID is not null;
        }

        private async Task<bool> _UpdateTestAppointment()
        {
            var UpdateTestAppointment = AutoMapperConfig.Mapper.Map<TestAppointmentForUpdateDTO>(this);
            return await clsTestAppointmentData.UpdateTestAppointment(TestAppointmentID ?? -1, UpdateTestAppointment);
        }

        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (await _AddNewTestAppointment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return await _UpdateTestAppointment();


            }

            return false;
        }

        public static async Task<bool> DeleteTestAppointment(int TestAppointmentID)
        {
            return await clsTestAppointmentData.DeleteTestAppointment(TestAppointmentID);
        }

        public static async Task<List<TestAppointmentFullDTO>> GetAllTestAppointments(enTestType TestTypeID, int LDLAppID)
        {
            return await clsTestAppointmentData.GetAllTestAppointments(TestTypeID, LDLAppID);
        }

        public static async Task<List<TestAppointmentPrefDTO>> GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, enTestType TestTypeID)
        {
            return await clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public async Task<List<TestAppointmentPrefDTO>> GetApplicationTestAppointmentsPerTestType(enTestType TestTypeID)
        {
            return await clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(LocalDrivingLicenseApplicationID ?? -1, (int)TestTypeID);
        }

        public static async Task<bool> LockAppointment(int TestAppointmentID)
        {
            return await clsTestAppointmentData.LockAppointment(TestAppointmentID);
        }

        public async Task<bool> LockAppointment()
        {
            return await clsTestAppointmentData.LockAppointment(TestAppointmentID ?? -1);
        }

        public static async Task<int?> GetTestID(int TestAppointmentID)
        {
            return await clsTestAppointmentData.GetTestID(TestAppointmentID);
        }

        public async Task<int?> GetTestID()
        {
            return await clsTestAppointmentData.GetTestID(TestAppointmentID ?? -1);
        } 
    }
}
