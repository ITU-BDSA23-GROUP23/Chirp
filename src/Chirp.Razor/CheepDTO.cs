using Chirp.Razor.Models;

public class CheepDTO
{
    public int Id { get; set; }
    public string Message { get; set; }
    public long TimeStamp { get; set; }
    public string AuthorName { get; set; }
    public string AuthorEmail { get; set; }


    public CheepDTO(Cheep cheap)
    {
        Message = cheap.Message;
        TimeStamp = ((DateTimeOffset)cheap.TimeStamp).ToUnixTimeMilliseconds();
        AuthorName = cheap.Author.Name;
        AuthorEmail = cheap.Author.Email;
    }
}
