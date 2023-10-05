using Microsoft.EntityFrameworkCore;
using Chirp.Razor.Models;

namespace Chirp.Razor
{
    public class ChirpDBContext : DbContext
    {
        public ChirpDBContext (DbContextOptions<ChirpDBContext> options)
            : base(options)
        {
        }

        public DbSet<Cheep> Cheeps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cheep>().ToTable("Cheeps");
        }
    }
}