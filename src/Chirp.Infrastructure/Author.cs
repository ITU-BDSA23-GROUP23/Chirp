using System.ComponentModel.DataAnnotations;

namespace Chirp.Infrastructure
{
    public class Author
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public IEnumerable<Cheep>? Cheeps { get; set; }
    }
}
