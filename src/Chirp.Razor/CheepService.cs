using Chirp.Razor;
using Chirp.Razor.Models;
using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


public interface ICheepService
{
    public List<Cheep> GetCheeps(int? page);
    // public List<DBFacade.CheepViewModel> GetCheepsFromAuthor(string author);
    public List<Cheep> GetCheepsFromAuthor(string authorName, int page);
}

public class CheepService : ICheepService
{
    private readonly int PageLimit = 32;
    private readonly ChirpDBContext context;
    public CheepService(ChirpDBContext context)
    {
        this.context = context;
        //context = new();
    }
    public List<Cheep> GetCheeps(int? page)
    {
        return context.Cheeps
            .OrderByDescending(t => t.TimeStamp)
            .Skip(CalculateSkipCheeps(page))
            .Take(PageLimit)
            .Include(c => c.Author)
            .ToList();
    }

    public List<Cheep> GetCheepsFromAuthor(string authorName, int page)
    {
        return context.Cheeps
            .Where(r => r.Author.Name == authorName)
            .OrderByDescending(t => t.TimeStamp)
            .Skip(CalculateSkipCheeps(page))
            .Take(PageLimit)
            .Include(c => c.Author)
            .ToList();
    }

    private int CalculateSkipCheeps(int? page)  
    {
         var skip = 0;
         if (page != null)
            skip = (int)((page - 1) * PageLimit);

        return skip;

    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
