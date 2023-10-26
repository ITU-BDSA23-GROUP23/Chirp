namespace Chirp.Infrastructure
{
    public class Author
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required IEnumerable<Cheep> Cheeps { get; set; }
    }
}
