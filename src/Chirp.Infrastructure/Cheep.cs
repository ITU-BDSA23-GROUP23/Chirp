namespace Chirp.Infrastructure
{
    public class Cheep {
        public int Id { get; set;}
        public required Author Author { get; set;}
        public string Message { get; set;}
        public DateTime TimeStamp { get; set;}
    }
}