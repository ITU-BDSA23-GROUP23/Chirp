namespace Chirp.Core
{
    public record AuthorDTO(string Name, string Email, ICollection<Guid>? Followers, ICollection<Guid>? Following);

}