using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NotesMicroservice.Models;

namespace NotesMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : Controller
    {
        private readonly IMongoCollection<NoteDBModel> _notesDB;
        public NotesController(IMongoDatabase mongoDatabase)
        {
            _notesDB = mongoDatabase.GetCollection<NoteDBModel>("Notes");
        }

        [HttpPost]
        public ActionResult<NoteModel> CreateNote(NoteModel note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdNote = _notesDB.InsertOneAsync(note.toNoteDBModl());

            return CreatedAtAction(nameof(GetNote), new { patientId = note.PatientId }, note);

        }

        [HttpGet]
        public async Task<ActionResult<string>> GetNote(int patientId)
        {
            NoteDBModel note = await _notesDB.Find(n => n.PatientId == patientId).FirstOrDefaultAsync();
            if (note == null)
            {
                return NotFound($"No note found for patient with id {patientId}");
            }
            return Ok(note.Notes);
        }


        [HttpPut]
        public async Task UpdatePatientNotes(NoteModel notes)
        {

            var filter = Builders<NoteDBModel>.Filter.Eq(n => n.PatientId, notes.PatientId);
            var update = Builders<NoteDBModel>.Update.Set(n => n.Notes, notes.Notes);
            _notesDB.UpdateOne(filter, update);
        }

        [HttpDelete]
        public void DeletePatientNotes(int patientId)
        {
            var filter = Builders<NoteDBModel>.Filter.Eq(r => r.PatientId, patientId);
            _notesDB.DeleteOne(filter);
        }
    }
}
