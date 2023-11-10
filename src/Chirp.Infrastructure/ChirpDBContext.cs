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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasIndex(a => a.Name).IsUnique();
            // modelBuilder.Entity<Author>().HasIndex(a => a.Email).IsUnique(); // makes no sense to have unique email when many of them would be null, as we currently don't have the email when creating Authors. 
            modelBuilder.Entity<Author>().ToTable("Authors");
            modelBuilder.Entity<Cheep>().ToTable("Cheeps");

            modelBuilder.Entity<Cheep>().Property(a => a.Message).HasMaxLength(160);
            modelBuilder.Entity<Author>().Property(a => a.Name).HasMaxLength(39); // Same as github max username length.


            // DOESN'T WORK!:
            //modelBuilder.Entity<Author>().HasIndex(a => a.Email).Email
            //modelBuilder.Entity<Author>().Property(a => a.Email).Email()

        }
    }
}
