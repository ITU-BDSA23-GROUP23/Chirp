using System.ComponentModel.DataAnnotations;

namespace Chirp.Infrastructure
{
    public class Author
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        public required ICollection<Cheep> Cheeps { get; set; }
    }
}
