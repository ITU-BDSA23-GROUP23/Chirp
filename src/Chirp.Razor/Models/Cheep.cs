namespace Chirp.Razor.Models
{
    public class Cheep {
        public int Id { get; set;}
        public Author Author { get; set;}
        public string Message { get; set;}
        public DateTime TimeStamp { get; set;}
    }
}