namespace Chirp.Core
{
    public class CheepDTO
    {
        public string Message { get; set; }
        public long TimeStamp { get; set; }
        public string? AuthorName { get; set; }
        public string? AuthorEmail { get; set; }


        public CheepDTO()
        {
            // Message = cheep.Message;
            // TimeStamp = ((DateTimeOffset)cheep.TimeStamp).ToUnixTimeMilliseconds();
            // AuthorName = cheep.Author.Name;
            // AuthorEmail = cheep.Author.Email;
        }

        public string GetMessage()
        {
            return Message;
        }

        public long GetTimeStamp()
        {
            return TimeStamp;
        }
    }
}
