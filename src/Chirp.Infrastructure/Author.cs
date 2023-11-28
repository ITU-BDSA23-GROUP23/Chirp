using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Chirp.Infrastructure
{
    public class Author
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public required virtual ICollection<Author> Followers { get; set; }

        public required virtual ICollection<Author> Following { get; set; }

        public ICollection<Reactions>? Reactions { get; set; }
        public required ICollection<Cheep> Cheeps { get; set; }
    }
}