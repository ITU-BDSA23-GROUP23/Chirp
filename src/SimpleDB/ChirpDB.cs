namespace SimpleDB;
using System.Numerics;
using System.Text.RegularExpressions;
using CsvHelper;
using System.Collections;
using CsvHelper;
using System.Collections;
using System.ComponentModel;
using CsvHelper.Configuration;
using System.Globalization;

public class ChirpDB : IDatabaseRepository<Cheep>
{
    public IEnumerable<Cheep> Read(int? limit = null)
    {   
        //this code is mostly from https://joshclose.github.io/CsvHelper/getting-started/
        using var reader = new StreamReader("../SimpleDB/chirp_cli_db.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Cheep>();
        var cheeps = new List<Cheep>();


        foreach (var record in records) 
        {
            cheeps.Add(record);
        } 
        return cheeps;
    }
    public void Store(Cheep cheep)
    {

         var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        // Don't write the header again.
        HasHeaderRecord = false,
    };
        var record = new List<Cheep>
        {
             cheep
        };

        //this code is mostly from https://joshclose.github.io/CsvHelper/getting-started/
        using var stream = File.Open("../SimpleDB/chirp_cli_db.csv", FileMode.Append);
        using var writer = new StreamWriter(stream);
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteRecords(record);
        };
    }
}
