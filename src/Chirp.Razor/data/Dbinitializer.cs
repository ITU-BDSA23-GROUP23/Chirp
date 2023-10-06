using Chirp.Razor;
using Chirp.Razor.Models;

namespace Chirp.Razor.data
{
    public static class DbInitializer
    {
        public static void Initialize(ChirpDBContext context)
        {
            // Look for any Authors.
            if (context.Authors.Any())
            {
                return;   // DB has been seeded
            }

            var authors = new Author[]
            {
                new Author{Name = "Testergut", Email = "TestergutTest@testing.test"},
                new Author{Name = "Testergut2", Email = "Testergut2Test@testing.test"}
            };

            context.Authors.AddRange(authors);
            context.SaveChanges();

            var cheepAuthor = context.Authors.FirstOrDefault(c => c.Name != null);

            var cheeps = new Cheep[]
            {
                new Cheep{Author = cheepAuthor, Message = "Test besked 1", TimeStamp = DateTime.Now},
                new Cheep{Author = cheepAuthor, Message = "Test besked 2", TimeStamp = DateTime.Now},
                new Cheep{Author = cheepAuthor, Message = "Test besked 3", TimeStamp = DateTime.Now},
                new Cheep{Author = cheepAuthor, Message = "Test besked 4", TimeStamp = DateTime.Now},
                new Cheep{Author = cheepAuthor, Message = "Test besked 5", TimeStamp = DateTime.Now},
            };

            context.Cheeps.AddRange(cheeps);
            context.SaveChanges();
        }
    }
}