namespace Chirp.Core
{   
    /// <summary>
    /// This is a data transfer objects which is used to transfor aurthor information to the outer parts of the code/onion
    /// </summary>
    public record ReactionDTO(Reactiontype Reactiontype, int Count);
    public record CheepDTO(string Message, string AuthorName, string TimeStamp, ICollection<ReactionDTO>? ReactionDTOs, ICollection<AuthorDTO>? Following);


    public enum Reactiontype
    {

        Like,
        Dislike,
        Love

    }
}
