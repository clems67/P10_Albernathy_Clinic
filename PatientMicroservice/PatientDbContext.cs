using Microsoft.EntityFrameworkCore;
using PatientMicroservice.Models;

namespace PatientMicroservice
{
    public class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options) { }
        public DbSet<PatientDBModel> Patients { get; set; }
    }
}
