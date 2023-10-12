using Azure;
using Chirp.Razor;
using Chirp.Razor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories;
using NuGet.Protocol.Plugins;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext dbContext;

    public CheepRepository(ChirpDBContext dbContext)
    {
        this.dbContext = dbContext;
    }


    public IEnumerable<CheepDTO> GetCheeps(int page = 1, int pageSize = 32, string? authorName = null) {
        IQueryable<Cheep> Cheeps;
        
        if (authorName != null) {
            Cheeps = dbContext.Cheeps.Where(c => c.Author.Name == authorName);
        } else {
            Cheeps = dbContext.Cheeps.Where(c => true);
        }

        Cheeps = Cheeps.OrderByDescending( t => t.TimeStamp)
            .Skip(CalculateSkippedCheeps(page, pageSize))
            .Take(pageSize)
            .Include(c => c.Author);
        
        return CheepsToCheepDTOs(Cheeps.ToList());
    }

    private int CalculateSkippedCheeps(int page, int pageSize) {
        return (page - 1) * pageSize;
    }

    private IEnumerable<CheepDTO> CheepsToCheepDTOs(List<Cheep> cheeps) {
        var cheepDTOs = new List<CheepDTO>();
        foreach (var cheep in cheeps) {
            cheepDTOs.Add(new CheepDTO(cheep));
        }

        return cheepDTOs;
    }

    // public void Add(Cheep entity)
    // {
    //     dbContext.Set<Cheep>().Add(entity);
    //     dbContext.SaveChanges();
    // }

    // public void Update(Cheep entity)
    // {
    //     dbContext.Set<Cheep>().Update(entity);
    //     dbContext.SaveChanges();
    // }

    // public void Remove(Cheep entity)
    // {
    //     dbContext.Set<Cheep>().Remove(entity);
    //     dbContext.SaveChanges();
    // }
}