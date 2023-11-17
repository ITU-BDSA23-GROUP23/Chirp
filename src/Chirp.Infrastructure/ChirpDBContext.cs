using Microsoft.EntityFrameworkCore;
using Chirp.Core;

namespace Chirp.Infrastructure
{
    public class ChirpDBContext : DbContext
    {
        public ChirpDBContext(DbContextOptions<ChirpDBContext> options)
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
        public DbSet<Reactions> Reactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasIndex(a => a.Name).IsUnique();
            modelBuilder.Entity<Author>().HasIndex(a => a.Email).IsUnique();
            modelBuilder.Entity<Author>().ToTable("Authors");
            modelBuilder.Entity<Cheep>().ToTable("Cheeps");
            modelBuilder.Entity<Reactions>().ToTable("Reactions");

            modelBuilder.Entity<Cheep>().Property(a => a.Message).HasMaxLength(160);
            modelBuilder.Entity<Author>().Property(a => a.Name).HasMaxLength(39); // Same as github max username length.
            modelBuilder.Entity<Reactions>().HasKey(r => new { r.ChirpId, r.AuthorId });

            // DOESN'T WORK!:
            //modelBuilder.Entity<Author>().HasIndex(a => a.Email).Email
            //modelBuilder.Entity<Author>().Property(a => a.Email).Email()

        }
    }
}
