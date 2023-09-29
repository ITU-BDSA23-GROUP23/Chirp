using System.Data;
using Microsoft.Data.Sqlite;
using Chirp.SQLite;
public class DBFacade
{
    private readonly string _CHIRPDBPATH = "/tmp/chirp.db";
    public DBFacade(string CHIRPDBPATH)
    {
        _CHIRPDBPATH = CHIRPDBPATH;
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
        using (var connection = new SqliteConnection($"Data Source={_CHIRPDBPATH}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            query = @"SELECT * FROM message ORDER by message.pub_date desc";
            command.CommandText = query;
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                cheeps.Add(new CheepViewModel(author, message, timestamp));
            }
        }
        return cheeps;
    }
}