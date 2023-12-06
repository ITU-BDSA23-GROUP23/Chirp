namespace Chirp.Infrastructure
{
    public class Reaction
    {
        public Guid ReactionId { get; set; }
        public string? ReactionType { get; set; }
        public Author? Author { get; set; }
        public Cheep? Cheep { get; set; }



    }
}