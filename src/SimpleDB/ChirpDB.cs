namespace SimpleDB;
using System.Numerics;
using System.Text.RegularExpressions;
using CsvHelper;
using System.Collections;
using System.ComponentModel;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;




public sealed class ChirpDB : IDatabaseRepository<Cheep>
{

    private string path;

    private ChirpDB()
    {

        path = getPath();

    }

    private static ChirpDB instance = null;

    public static ChirpDB Instance
    {

        get
        {
            if (instance == null)
            {
                instance = new ChirpDB();
            }
            return instance;
        }
    }



    public IEnumerable<Cheep> Read(int? limit = null)
    {
        //this code is mostly from https://joshclose.github.io/CsvHelper/getting-started/
        using var reader = new StreamReader(path);
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
        using var stream = File.Open(path, FileMode.Append);
        using var writer = new StreamWriter(stream);
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteRecords(record);
        };
    }

    //Returns different path, due to folder structure changes when publishing
    private string getPath()
    {
        var path = "chirp_cli_db.csv";

        if (File.Exists("src/SimpleDB/chirp_cli_db.csv"))
        {
            path = "src/SimpleDB/chirp_cli_db.csv";
        }

        return path;
    }
}