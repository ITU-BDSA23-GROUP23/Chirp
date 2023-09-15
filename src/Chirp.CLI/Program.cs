namespace Chirp.CLI;
using System.Numerics;
using System.Text.RegularExpressions;
using CsvHelper;
using System.Collections;
using CommandLine;
using System.ComponentModel;
using CsvHelper.Configuration;
using System.Globalization;
using SimpleDB;


public class Program
{
    public class Options
    {
        [Option("read", Group = "action", Required = false, HelpText = "Reads all cheeps")]
        public bool Read { get; set; }

        [Option("cheep", Group = "action", Required = false, HelpText = "To send a cheep, write: run --cheep \"<message>\" ")]
        public string? cheepMessage { get; set; }
    }
    //Command line parser, external library: https://github.com/commandlineparser/commandline

    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
            {
                if (o.Read)
                {
                    Read();
                }
                if (o.cheepMessage != null) 
                {
                    SaveCheep(o.cheepMessage);
                } else {
                    args = new[] { "--help"};
                }

            });
    }
    
    public static void Read()
    {

        var records = SimpleDB.ChirpDB.Instance.Read(1);
        var cheeps = new List<Cheep>();
        
        foreach (var record in records) 
        {
            cheeps.Add(new Cheep(record.Id, record.Message, record.Time));
        }

        UserInterface.PrintCheeps(cheeps);



    }

    public static void SaveCheep(string message)
    {
        string author = Environment.UserName; //Takes username from computer
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        Console.WriteLine(author + ",\"" + message + "\"," + timestamp);

        var db = SimpleDB.ChirpDB.Instance;
        var Cheep = new SimpleDB.Cheep {Id = author, Message = message, Time = timestamp};

        db.Store(Cheep);
    }
}