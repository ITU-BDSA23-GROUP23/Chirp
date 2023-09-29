public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{

    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    // These would normally be loaded from a database for example
    private readonly string _databasePath;
    public CheepService(string databasePath)
    {
        _databasePath = databasePath;
    }

    public List<CheepViewModel> GetCheeps()
    {
        return GetCheepsFromQuery("SELECT * FROM message");
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        string query = $"SELECT * FROM message WHERE author_id = (SELECT user_id FROM user WHERE username = '{author}');";
        return GetCheepsFromQuery(query);
    }
    private List<CheepViewModel> GetCheepsFromQuery(string query)
    {
        List<CheepViewModel> cheeps = new List<CheepViewModel>();
        using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
        {
            connection.Open();

            var command = cnnection.CreateCommand();
            query = @"SELECT * FROM message ORDER by message.pub_date desc";
            command.CommandText = query;
        }
        cheeps.Add(new CheepViewModel(author, message, timestamp));
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
