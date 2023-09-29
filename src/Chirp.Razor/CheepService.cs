using System.Runtime.InteropServices;

public record CheepViewModel(string Author, string Message, long Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private static HttpClient? client;

    public CheepService() {
                // port: 5248
        var baseURL = "https://bdsagroup23chirpremotedb.azurewebsites.net/";
        client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        var CheepTask = new Task(async () => 
        {
           List<CheepViewModel>? _nullable_cheeps = await client.GetFromJsonAsync<List<CheepViewModel>>("Cheeps");
           if (_nullable_cheeps != null) {
                _cheeps = _nullable_cheeps;
           }
        });
        CheepTask.Start();
        CheepTask.Wait();
    }

    // These would normally be loaded from a database for example
    private static List<CheepViewModel> _cheeps = new();

    public List<CheepViewModel> GetCheeps()
    {
        return _cheeps;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return _cheeps.Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
