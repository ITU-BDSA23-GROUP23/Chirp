using Chirp.Core;

namespace Chirp.Infrastructure
{
        //temp

        public interface IAuthorRepository
        {
                Task<AuthorDTO?> FindAuthorByName(string Name);
                Task<AuthorDTO?> FindAuthorByEmail(string Email);
                void CreateAuthor(AuthorDTO author);
                Task<long> GetCheepAmount(string authorName);

        }
}