using Chirp.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICheepService : IDisposable
{
    Task<IEnumerable<CheepDTO>> GetCheeps(int page);
    Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string authorName, int page);
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository cheepRepository;

    public CheepService(ICheepRepository cheepRepository)
    {
        this.cheepRepository = cheepRepository;
    }

    public async Task<IEnumerable<CheepDTO>> GetCheeps(int page)
    {
        return await cheepRepository.GetCheeps(page);
    }

    public async Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string authorName, int page)
    {
        return await cheepRepository.GetCheeps(page, authorName: authorName);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

    public void Dispose()
    {
        // Implement Dispose method here if necessary.
    }
}
