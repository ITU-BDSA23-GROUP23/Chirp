using System.ComponentModel.DataAnnotations;

namespace Chirp.Infrastructure
{
    public class Author
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public ICollection<Author>? Followers { get; set; }

        public ICollection<Author>? Following { get; set; }

        public ICollection<Reactions>? Reactions { get; set; }
        public required ICollection<Cheep> Cheeps { get; set; }
    }
}