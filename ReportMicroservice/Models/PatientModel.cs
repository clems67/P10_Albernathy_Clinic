using System.ComponentModel.DataAnnotations;

namespace ReportMicroservice.Models
{
    public class PatientModel
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
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
        [RegularExpression(@"^\+\d{6,12}$", ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; }
    }
}
