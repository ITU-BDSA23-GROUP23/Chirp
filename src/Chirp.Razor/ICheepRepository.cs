using Chirp.Razor.Models;
namespace Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories
{
    public interface ICheepRepository<Cheep>
    {
        Cheep GetById(int id); //Perhaps we can call this SELECT
        IEnumerable<Cheep> GetAll();
        void Add(Cheep entity);
        void Update(Cheep entity);
        void Remove(Cheep entity);
    }
}
