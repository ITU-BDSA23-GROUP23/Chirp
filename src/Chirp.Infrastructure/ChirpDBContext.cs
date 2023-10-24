using Microsoft.EntityFrameworkCore;
using Chirp.Core;

namespace Chirp.Infrastructure
{
    public class ChirpDBContext : DbContext
    {
        public ChirpDBContext (DbContextOptions<ChirpDBContext> options)
            : base(options)
        {
        }

        // public string DbPath { get; }

        // public ChirpDBContext() {
        //     var folder = Environment.SpecialFolder.LocalApplicationData;
        //     var path = Environment.GetFolderPath(folder);
        //     Console.WriteLine($"chirp.db file path: {path}");
        //     DbPath = Path.Join(path, "chirp.db");
        // }
        // protected override void OnConfiguring(DbContextOptionsBuilder options)
        // => options.UseSqlite($"Data Source={DbPath}");

        public DbSet<Author> Authors { get; set; }
        public DbSet<Cheep> Cheeps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Author>().HasIndex(c => c.Email).IsUnique();
            modelBuilder.Entity<Author>().ToTable("Authors");
            modelBuilder.Entity<Cheep>().ToTable("Cheeps");
        }
    }
}
