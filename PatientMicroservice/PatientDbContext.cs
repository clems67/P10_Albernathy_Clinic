using Microsoft.EntityFrameworkCore;

namespace PatientMicroservice
{
    public class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options) { }
        public DbSet<PatientModel> Patients { get; set; }
    }
}
