﻿namespace DataLayerCore.Driver
{
    public class DriverFullDTO
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte NumberofActiveLicenses { get; set; }

    }
}
