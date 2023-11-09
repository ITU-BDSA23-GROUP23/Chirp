namespace Chirp.Core
{
    public record ReactionDTO(Reactiontype Reactiontype, int Count);
    public record CheepDTO(string Message, string AuthorName, string TimeStamp, ICollection<ReactionDTO>? ReactionDTOs);

    public enum Reactiontype
    {

        Like,
        Dislike,
        Love

    }
}
