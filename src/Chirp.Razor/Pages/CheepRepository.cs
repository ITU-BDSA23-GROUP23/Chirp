using Chirp.Razor.Models;
namespace Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories
{
    public interface ICheepRepository
    {
        public List<Cheep> GetCheeps();
    }
}
