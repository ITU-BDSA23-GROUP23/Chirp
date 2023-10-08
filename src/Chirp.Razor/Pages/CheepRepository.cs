using Chirp.Razor;
using Chirp.Razor.Models;
using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories;

public class CheepRepository : ICheepRepository<Cheep>
{
    private readonly ChirpDBContext _dbContext;

    public CheepRepository(ChirpDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Cheep? GetById(int id)
    {
        return _dbContext.Set<Cheep>().Find(id);
    }

    public IEnumerable<Cheep> GetAll()
    {
        return _dbContext.Set<Cheep>().ToList();
    }

    public void Add(Cheep entity)
    {
        _dbContext.Set<Cheep>().Add(entity);
        _dbContext.SaveChanges();
    }

    public void Update(Cheep entity)
    {
        _dbContext.Set<Cheep>().Update(entity);
        _dbContext.SaveChanges();
    }

    public void Remove(Cheep entity)
    {
        _dbContext.Set<Cheep>().Remove(entity);
        _dbContext.SaveChanges();
    }
}
