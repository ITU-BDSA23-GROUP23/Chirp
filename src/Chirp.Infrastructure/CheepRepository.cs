using Microsoft.EntityFrameworkCore;
using Chirp.Core;
using Chirp.Infrastructure;
using System.Diagnostics.CodeAnalysis;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext dbContext;

    public CheepRepository(ChirpDBContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public CheepDTO CreateCheep(CheepDTO cheepDTO)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CheepDTO>> GetCheeps(int page = 1, int pageSize = 32, string? authorName = null)
    {
        IQueryable<Cheep> Cheeps;

        if (authorName != null)
        {
            Cheeps = dbContext.Cheeps.Where(c => c.Author.Name == authorName);
        }
        else
        {
            Cheeps = dbContext.Cheeps.Where(c => true);
        }

        Cheeps = Cheeps.OrderByDescending(t => t.TimeStamp)
            .Skip(CalculateSkippedCheeps(page, pageSize))
            .Take(pageSize)
            .Include(c => c.Author);

        return await CheepsToCheepDTOs(Cheeps.ToListAsync());
    }

    private int CalculateSkippedCheeps(int page, int pageSize)
    {
        return (page - 1) * pageSize;
    }

    private async Task<IEnumerable<CheepDTO>> CheepsToCheepDTOs(Task<List<Cheep>> cheeps)
    {
        var cheepDTOs = new List<CheepDTO>();
        foreach (var cheep in await cheeps)
        {
            cheepDTOs.Add(new CheepDTO(cheep.Message, cheep.Author.Name, cheep.TimeStamp));
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
