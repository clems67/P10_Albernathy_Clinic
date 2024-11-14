using MongoDB.Bson;

namespace NotesMicroservice.Models
{
    public class NoteDBModel
    {
        public ObjectId _id { get; set; }
        public int PatientId { get; set; }
        public string Notes { get; set; }

        public NoteModel toNoteModel()
        {
            return new NoteModel
            {
                PatientId = this.PatientId,
                Notes = this.Notes,
            };
        }
    }
}
