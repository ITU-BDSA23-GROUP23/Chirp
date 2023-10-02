using Chirp.Razor;

public interface ICheepService
{
    public List<DBFacade.CheepViewModel> GetCheeps();
    public List<DBFacade.CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    // These would normally be loaded from a database for example
    private static readonly string CHIRPDBPATH = "/tmp/chirp.db";
    private readonly DBFacade dBFacade = new(CHIRPDBPATH);

    public List<DBFacade.CheepViewModel> GetCheeps()
    {
        return dBFacade.GetCheeps();
    }

    public List<DBFacade.CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return dBFacade.GetCheepsFromAuthor(author);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
