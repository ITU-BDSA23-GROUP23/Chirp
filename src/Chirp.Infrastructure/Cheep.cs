using System.ComponentModel.DataAnnotations;

namespace Chirp.Infrastructure
{
    public class Cheep
    {
        public Guid Id { get; set; }

        public required Author Author { get; set; }

        [MaxLength(128)]
        [MinLength(1)]
        public required string Message { get; set; }

        public required DateTime TimeStamp { get; set; }
    }
}