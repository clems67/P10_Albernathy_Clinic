using MongoDB.Bson;

namespace NotesMicroservice.Models
{
    public class NoteDBModel
    {
        public ObjectId _id { get; set; }
        public int PatientId { get; set; }
        public DateTime DateTime { get; set; }
        public string Notes { get; set; }
    }
}
