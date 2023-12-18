using Microsoft.EntityFrameworkCore;
using Chirp.Core;

namespace Chirp.Infrastructure
{
    /// <summary>
    /// this class is our database context https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
    /// We give the context our entity classes that make up the model here Authors, cheep and reactions
    /// </summary>
    public class ChirpDBContext : DbContext
    {
        public ChirpDBContext(DbContextOptions<ChirpDBContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Cheep> Cheeps { get; set; }
        public DbSet<Reaction> Reactions { get; set; }

        /// <summary>
        /// https://learn.microsoft.com/en-us/ef/core/modeling/
        /// Here we configure specific rules for the model and DB without change the entities themself
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Author>().HasIndex(a => a.Name).IsUnique();
            modelBuilder.Entity<Author>().HasMany(f => f.Followers).WithMany(f => f.Following);
            modelBuilder.Entity<Author>().HasMany(r => r.Reactions).WithOne(r => r.Author);
            // modelBuilder.Entity<Author>().HasMany(f => f.Following);
            // modelBuilder.Entity<Author>().HasIndex(a => a.Email).IsUnique(); // makes no sense to have unique email when many of them would be null, as we currently don't have the email when creating Authors. 
            modelBuilder.Entity<Author>().ToTable("Authors");
            modelBuilder.Entity<Cheep>().ToTable("Cheeps");
            modelBuilder.Entity<Reaction>().ToTable("Reactions");

            modelBuilder.Entity<Cheep>().Property(a => a.Message).HasMaxLength(160);
            modelBuilder.Entity<Author>().Property(a => a.Name).HasMaxLength(39); // Same as github max username length.
            


            modelBuilder.Entity<Author>().Property(a => a.Email).HasAnnotation("RegularExpression", "[.+@.+]");


        }
    }
}
