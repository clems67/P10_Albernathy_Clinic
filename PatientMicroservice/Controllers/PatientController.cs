using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PatientMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : Controller
    {
        private readonly PatientDbContext _context;

        public PatientController(PatientDbContext context)
        {
            _context = context;
        }

        [HttpPost(Name = "Create Patient")]
        public void CreatePatient([FromBody] PatientModel patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }

        [HttpGet(Name = "Get Patients")]
        public List<PatientModel> GetPatients()
        {
            return _context.Patients.ToList();
        }

        [HttpPut(Name = "Update Patient")]
        public void UpdatePatient([FromBody] PatientModel patient)
        {
            _context.Patients.Update(patient);
            _context.SaveChanges();
        }

        [HttpDelete(Name = "Delete Patient")]
        public void DeletePatient(int id)
        {
            _context.Patients.Where(p => p.Id == id).ExecuteDelete();
            _context.SaveChanges();
        }
    }
}
