using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NotesMicroservice.Models;

namespace NotesMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : Controller
    {
        private readonly IMongoCollection<NoteDBModel> _notes;
        public NotesController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("NotesDB");
            _notes = database.GetCollection<NoteDBModel>("notesTable");
        }

        public class BasicNoteModel{
            public int patientId { get; set; }
            public string notes { get; set; }
        }

        [HttpPost]
        public ActionResult<BasicNoteModel> CreateNote(BasicNoteModel note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var noteToAdd = new NoteDBModel
            {
                PatientId = note.patientId,
                Notes = note.notes,
                DateTime = DateTime.Now,
            };

            _notes.InsertOne(noteToAdd);

            return CreatedAtAction(nameof(GetNotes), new { patientId = note.patientId }, note);
        }

        [HttpGet]
        public ActionResult<List<NoteDBModel>> GetNotes(int patientId)
        {
            var filter = Builders<NoteDBModel>.Filter.Eq(n => n.PatientId, patientId);
            List<NoteDBModel> notes = _notes.Find(filter).ToList();

            if (notes == null)
            {
                return NotFound($"No note found for patient with id {patientId}");
            }

            return Ok(notes);
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePatientNotes(BasicNoteModel notes)
        {
            var filter = Builders<NoteDBModel>.Filter.Eq(n => n.PatientId, notes.patientId);
            var update = Builders<NoteDBModel>.Update.Set(n => n.Notes, notes.notes);
            var result = _notes.UpdateOne(filter, update);
            if(result.ModifiedCount == 1) { return Ok(result); }
            else { return Problem("unexpected error, ressource not updated"); }
        }
            
        [HttpDelete]
        public void DeletePatientNotes(int patientId)
        {
            var filter = Builders<NoteDBModel>.Filter.Eq(r => r.PatientId, patientId);
            _notes.DeleteOne(filter);
        }
    }
}
