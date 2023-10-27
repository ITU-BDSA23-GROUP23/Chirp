using System.ComponentModel.DataAnnotations;

namespace Chirp.Infrastructure
{
    public class Cheep
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Author Author { get; set; }

        [Required]
        [MaxLength(128)]
        [MinLength(1)]
        public string Message { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }
    }
}