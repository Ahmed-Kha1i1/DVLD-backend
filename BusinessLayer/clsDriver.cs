﻿using BusinessLayer.InternationalLicense;
using BusinessLayer.License;
using BusinessLayerCore;
using DataLayerCore.Driver;
using DataLayerCore.InternationalLicense;
using DataLayerCore.License;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessLayer
{

    public class clsDriver
    {
        public enum enMode
        {
            AddNew,
            Update
        }
        public enMode Mode = enMode.AddNew;

        public clsPerson? PersonInfo;

        public int? DriverID { set; get; }
        public int? PersonID { set; get; }
        public int? CreatedByUserID { set; get; }
        public DateTime CreatedDate { get; set; }


        public clsDriver()

        {
            this.DriverID = null;
            this.PersonID = null;
            this.CreatedByUserID = null;
            this.CreatedDate = DateTime.Now;

            Mode = enMode.AddNew;

        }

        private clsDriver(DriverDTO Driver)

        {
            this.DriverID = Driver.DriverID;
            this.PersonID = Driver.PersonID;
            this.CreatedByUserID = Driver.CreatedByUserID;
            this.CreatedDate = Driver.CreatedDate;

            Mode = enMode.Update;
        }

        private static async Task<clsDriver> CreateAsync(DriverDTO DriverDtO)
        {
            Task<clsPerson?> Person = clsPerson.Find(DriverDtO.PersonID);

            clsDriver Driver = new clsDriver(DriverDtO);


            Driver.PersonInfo = await Person;

            return Driver;
        }

        public static async Task<clsDriver?> FindByDriverID(int DriverID)
        {

            var Driver = await clsDriverData.GetDriverInfoByDriverID(DriverID);
            if (Driver is not null)

                return await CreateAsync(Driver);
            else
                return null;

        }

        public static async Task<clsDriver?> FindByPersonID(int PersonID)
        {


            var Driver = await clsDriverData.GetDriverInfoByPersonID(PersonID);
            if (Driver is not null)

                return await CreateAsync(Driver);
            else
                return null;


        }
        private async Task<bool> _AddNewDriver() 
        {
            var NewDriver = AutoMapperConfig.Mapper.Map<DriverForCreateDTO>(this);
            this.DriverID = await clsDriverData.AddNewDriver(NewDriver);



            return (this.DriverID is not null);
        }


        private async Task<bool> _UpdateDriver()
        {
            var UpdateDriver = AutoMapperConfig.Mapper.Map<DriverForUpdateDTO>(this);
            return await clsDriverData.UpdateDriver(DriverID ?? -1, UpdateDriver);
        }

       

        public static async Task<List<DriverFullDTO>> GetAllDrivers()
        {
            return await clsDriverData.GetAllDrivers();

        }

        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewDriver())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return await _UpdateDriver();

            }

            return false;
        }

        public static async Task<List<LicenseInfoDTO>>GetLicenses(int DriverID)
        {
            return await clsLicense.GetDriverLicenses(DriverID);
        }

        public static async Task<List<InternationalLicenseDTO>> GetInternationalLicenses(int DriverID)
        {
            return await clsInternationalLicense.GetInternationalLicenses(DriverID);
        }

    }
}