using System.ComponentModel.DataAnnotations;

namespace Chirp.Infrastructure
{
    /// <summary>
    /// A cheep is a message posted by an author, who has been authenticated via github. It has a unique id, a string containing the message, and a timestamp.
    /// It also has a reference to the Author that posted it.
    /// </summary>
    public class Cheep
    {
        public Guid Id { get; set; }

        public required Author Author { get; set; }

        public required string Message { get; set; }

        public required DateTime TimeStamp { get; set; }

        public ICollection<Reaction>? Reactions { get; set; }

        public string ToString()
        {
            return $"Cheep: id:{Id}, Message: {Message} Timestamp: {TimeStamp}";
        }
    }

    
}