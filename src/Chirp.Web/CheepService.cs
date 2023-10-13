using Chirp.Core;


public interface ICheepService : IDisposable
{
    public Task<IEnumerable<CheepDTO>> GetCheeps(int page);
    // public List<DBFacade.CheepViewModel> GetCheepsFromAuthor(string author);
    public Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string authorName, int page);
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository cheepRepository;
    public CheepService(ICheepRepository cheepRepository)
    {
        this.cheepRepository = cheepRepository;
        //context = new();
    }
    public async Task<IEnumerable<CheepDTO>> GetCheeps(int page)
    {
        return await cheepRepository.GetCheeps(page);
    }

    public async Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string authorName, int page)
    {
        return await cheepRepository.GetCheeps(page, authorName: authorName);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp) //Converts Unix timestamp to a date in MM/dd/yy format
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
