namespace Chirp.Infrastructure
{
    public class Cheep
    {
        public Guid Id { get; set; }
        public required Author Author { get; set; }
        public required string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}