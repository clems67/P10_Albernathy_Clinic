using System.ComponentModel.DataAnnotations;

namespace PatientMicroservice.Models
{
    public class PatientDBModel
    {
        [Key]
        public int Id { get; set; }
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
        public PatientModel toPatientModel(PatientDBModel baseModel)
        {
            return new PatientModel
            {
                FirstName = baseModel.FirstName,
                LastName = baseModel.LastName,
                BirthDate = baseModel.BirthDate,
                Sex = baseModel.Sex,
                Adress = baseModel.Adress,
                Phone = baseModel.Phone,
            };
        }
    }
}
