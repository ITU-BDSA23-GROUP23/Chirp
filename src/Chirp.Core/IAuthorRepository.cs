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
                void CreateAuthor(CreateAuthorDTO author);
                Task<long> GetCheepAmount(string authorName);
                Task UnfollowAuthor(AuthorDTO self, AuthorDTO other);
                Task FollowAuthor(AuthorDTO self, AuthorDTO other);
                Task<IEnumerable<AuthorDTO>> GetFollowers(string authorName);
                Task<IEnumerable<AuthorDTO>> GetFollowing(string authorName);
                Task DeleteAuthor(string authorName);
                Task RemoveFollowers(IEnumerable<AuthorDTO> result);
                Task RemoveFollowing(IEnumerable<AuthorDTO> result);
        }
}