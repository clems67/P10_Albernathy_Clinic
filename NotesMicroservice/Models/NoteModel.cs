namespace NotesMicroservice.Models
{
    public class NoteModel
    {
        public int PatientId { get; set; }
        public string Notes { get; set; }

        public NoteDBModel toNoteDBModl()
        {
            return new NoteDBModel
            {
                PatientId = this.PatientId,
                Notes = this.Notes,
            };
        }
    }
}