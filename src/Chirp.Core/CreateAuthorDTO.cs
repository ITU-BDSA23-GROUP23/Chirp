namespace Chirp.Core
{
    /// <summary>
    /// This is a data transfer object which is used from the outer part of the code to be send back to the repositories when we create a Author
    /// </summary>
    public record CreateAuthorDTO(string Name, string Email);
}