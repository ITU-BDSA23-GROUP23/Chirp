namespace Chirp.Razor.Models
{

    public class AuthorDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public AuthorDTO(string Name, string Email)
        {
            this.Name = Name;
            this.Email = Email;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetEmail()
        {
            return Email;
        }

    }
}