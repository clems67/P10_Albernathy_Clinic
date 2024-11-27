using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientMicroservice.Models;

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
        public ActionResult CreatePatient([FromBody] PatientModel patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patientToAdd = patient.toPatientDBModel();
            var patientAdded = _context.Patients.Add(patientToAdd);
            _context.SaveChanges();
            return Ok(patientAdded.Entity.Id);
        }

        [HttpGet(Name = "Get Patients")]
        public ActionResult<PatientDBModel> GetPatientInformations(int patientId)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.Id == patientId);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        [HttpPut(Name = "Update Patient")]
        public ActionResult UpdatePatient([FromBody] PatientDBModel patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patientToUpdate = patient;
            _context.Patients.Update(patientToUpdate);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete(Name = "Delete Patient")]
        public ActionResult DeletePatient(int id)
        {
            _context.Patients.Where(p => p.Id == id).ExecuteDelete();
            _context.SaveChanges();
            return Ok();
        }
    }
}
