using Chirp.Razor;
using Chirp.Razor.Models;
using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


public interface ICheepService : IDisposable
{
    public IEnumerable<CheepDTO> GetCheeps(int page);
    // public List<DBFacade.CheepViewModel> GetCheepsFromAuthor(string author);
    public IEnumerable<CheepDTO> GetCheepsFromAuthor(string authorName, int page);
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository cheepRepository;
    public CheepService(ICheepRepository cheepRepository)
    {
        this.cheepRepository = cheepRepository;
        //context = new();
    }
    public IEnumerable<CheepDTO> GetCheeps(int page)
    {
        return cheepRepository.GetCheeps(page);
    }

    public IEnumerable<CheepDTO> GetCheepsFromAuthor(string authorName, int page)
    {
        return cheepRepository.GetCheeps(page, authorName: authorName);
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
