namespace SimpleDB;
public class ChirpDB : IDatabaseRepository<Cheep>
{
    public IEnumerable<T> Read(int? limit = null)
    {
        using var reader = new StreamReader("chirp_cli_db.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Cheep>();
        foreach (var record in records) 
        {
            Console.WriteLine($"{record.Id} @ {record.Name}: {record.Time}");
        } 
    }
    public void Store(T record)
    {
        string author = Environment.UserName; //Takes username from computer
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string cheepString;
        
        ArrayList cheepList = new ArrayList();
        
       
        foreach (String word in message)
        {
            cheepList.Add(word);
        }
        cheepString = string.Join(" ", cheepList.ToArray());
        Console.WriteLine(author + ",\"" + cheepString + "\"," + timestamp);

        var record = new List<Cheep>
        {
            new Cheep {Id = author, Name = cheepString, Time = timestamp}
        };

         var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        // Don't write the header again.
        HasHeaderRecord = false,
    };
        using var stream = File.Open("chirp_cli_db.csv", FileMode.Append);
        using var writer = new StreamWriter(stream);
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteRecords(record);
        };
    }
}
