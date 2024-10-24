using CandidateManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CandidateManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Candidate> Candidates { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>()
                .HasIndex(c => c.Email)
                .IsUnique(); // Email must be unique
        }
    }
}
