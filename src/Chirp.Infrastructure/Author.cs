using System.ComponentModel.DataAnnotations;

namespace Chirp.Infrastructure
{
    /// <summary>
    /// An author is a user of the application, who has tried to post at least one cheep, while loggen in with github. They have a unique id, a name, and an email address.
    /// They also have a collection of cheeps that they have posted.
    /// At this point in time, the email address is not used for anything, but it is there for future use.
    /// </summary>
    public class Author
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public required ICollection<Cheep> Cheeps { get; set; }
    }
}