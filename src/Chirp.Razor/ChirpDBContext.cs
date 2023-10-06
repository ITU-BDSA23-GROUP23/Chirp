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

        // public string DbPath { get; }

        // public ChirpDBContext() {
        //     var folder = Environment.SpecialFolder.LocalApplicationData;
        //     var path = Environment.GetFolderPath(folder);
        //     Console.WriteLine($"chirp.db file path: {path}");
        //     DbPath = Path.Join(path, "chirp.db");
        // }
        // protected override void OnConfiguring(DbContextOptionsBuilder options)
        // => options.UseSqlite($"Data Source={DbPath}");

        public DbSet<Cheep> Cheeps { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cheep>().ToTable("Cheeps");
            modelBuilder.Entity<Author>().ToTable("Authors");
        }
    }
}