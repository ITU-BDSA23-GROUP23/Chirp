using Chirp.Core;

namespace Chirp.Infrastructure
{

        public interface IAuthorRepository
        {
                AuthorDTO FindAuthorByName(string Name);
                AuthorDTO FindAuthorByEmail(string Email);
                void CreateAuthor(AuthorDTO author);
        }
}