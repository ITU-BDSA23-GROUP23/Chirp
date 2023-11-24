using Chirp.Core;

namespace Chirp.Infrastructure
{
        /// <summary>
        /// We use a repository pattern to abstract away the database. This interface defines the Author specific methods that we need in our repository.
        /// </summary>
        public interface IAuthorRepository
        {
                Task<AuthorDTO?> FindAuthorByName(string Name);
                Task<AuthorDTO?> FindAuthorByEmail(string Email);
                void CreateAuthor(AuthorDTO author);
                Task<long> GetCheepAmount(string authorName);

        }
}