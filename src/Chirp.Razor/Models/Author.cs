namespace Chirp.Razor.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public IEnumerable<Cheep>? Cheeps { get; set; }
    }
}