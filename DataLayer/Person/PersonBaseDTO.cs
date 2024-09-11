﻿namespace DataLayerCore.Person
{
    public abstract class PersonBaseDTO
    {
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string? ImagePath { get; set; }
    }
}