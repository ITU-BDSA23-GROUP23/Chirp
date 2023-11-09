using System.ComponentModel.DataAnnotations;

namespace Chirp.Infrastructure
{
    public class Cheep
    {
        public Guid Id { get; set; }

        public required Author Author { get; set; }

        public required string Message { get; set; }

        public required DateTime TimeStamp { get; set; }

        public ICollection<Reactions>? Reactions { get; set; }
    }
}