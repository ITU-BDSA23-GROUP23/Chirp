using Chirp.Core;

namespace Chirp.Infrastructure
{
        //temp

        public interface IAuthorRepository
        {
                Task<AuthorDTO?> FindAuthorByName(string Name);
                Task<AuthorDTO?> FindAuthorByEmail(string Email);
                void CreateAuthor(CreateAuthorDTO author);
                Task<long> GetCheepAmount(string authorName);
                Task UnfollowAuthor(AuthorDTO self, AuthorDTO other);
                Task FollowAuthor(AuthorDTO self, AuthorDTO other);
        }
}