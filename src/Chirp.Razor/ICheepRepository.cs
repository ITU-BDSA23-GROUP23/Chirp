using Chirp.Razor.Models;
namespace Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories
{
    public interface ICheepRepository
    {
        IEnumerable<CheepDTO> GetCheeps(int page = 1, int pageSize = 32, string? authorName = null);
        CheepDTO CreateCheep(CheepDTO cheepDTO);

        // void Add(Cheep entity);
        // void Update(Cheep entity);
        // void Remove(Cheep entity);
    }
}
