﻿using System.ComponentModel.DataAnnotations;

namespace PatientMicroservice.Models
{
    public class PatientModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        private char _Sex { get; set; }
        public char Sex
        {
            get
            {
                return _Sex;
            }
            set
            {
                if (value == 'M' || value == 'F')
                {
                    _Sex = value;
                }
                else
                {
                    throw new ArgumentException("Sex should be either 'M' or 'F'.");
                }
            }
        }
        public string Adress { get; set; }

        [RegularExpression(@"^\+?(\-|\d){6,16}$", ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; }
        
        public PatientDBModel toPatientDBModel()
        {
            return new PatientDBModel()
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                BirthDate = this.BirthDate,
                Sex = this.Sex,
                Adress = this.Adress,
                Phone = this.Phone
            };
        }
    }
}
