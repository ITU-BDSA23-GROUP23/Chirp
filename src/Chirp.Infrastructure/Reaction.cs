namespace Chirp.Infrastructure
{   
    /// <summary>
    /// An Reaction is a represenation of an reaction the user has given to a cheep, it contains a id, type, the aurhor and the cheep that was reactec too
    /// </summary>
    public class Reaction
    {
        public Guid ReactionId { get; set; }
        public string? ReactionType { get; set; }
        public Author? Author { get; set; }
        public Cheep? Cheep { get; set; }



    }
}