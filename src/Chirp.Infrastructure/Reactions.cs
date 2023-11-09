namespace Chirp.Infrastructure
{
    public class Reactions
    {

        public int ChirpId { get; set; }
        public Guid AuthorId { get; set; }
        public string? ReactionType { get; set; }
        public Author? Author { get; set; }
        public Cheep? Cheep { get; set; }



    }
}