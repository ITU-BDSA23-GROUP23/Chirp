using Chirp.Razor.Models;
namespace Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories
{

        public interface IAuthorRepository
        {
                AuthorDTO FindAuthorByName(string Name);
                AuthorDTO FindAuthorByEmail(string Email);
                void CreateAuthor(AuthorDTO author);
        }
}