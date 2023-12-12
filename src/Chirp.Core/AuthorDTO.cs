namespace Chirp.Core
{
    /// <summary>
    /// This is a data transfer object which is used to transfor aurthor information to the outer parts of the code/onion
    /// </summary>
    public record AuthorDTO(string Name, string Email, ICollection<Guid>? Followers, ICollection<Guid>? Following, Guid Id);

}
