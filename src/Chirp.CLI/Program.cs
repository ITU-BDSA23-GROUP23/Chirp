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
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;


//This is for a test

public class Program
{
    public class Options
    {
        [Option("read", Group = "action", Required = false, HelpText = "Reads all cheeps")]
        public bool Read { get; set; }

        [Option("lines", Group = "action", Required = false, HelpText = "Specify amount of lines to read")]
        public int? lines { get; set; }

        [Option("cheep", Group = "action", Required = false, HelpText = "To send a cheep, write: run --cheep \"<message>\" ")]
        public string? cheepMessage { get; set; }
    }
    //Command line parser, external library: https://github.com/commandlineparser/commandline

    static async Task Main(string[] args)
    {
        int? lines = null;
        var cheepMessage = "";
        bool readoption = false;
        bool storeoption = false;

   
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options> ( o =>
            {
                if (o.Read)
                {
                    readoption = true;
                    lines = o.lines;
                }
                if (o.cheepMessage != null)
                {
                    storeoption = true;
                    cheepMessage = o.cheepMessage;
                }
                else
                {
                    args = new[] { "--help" };
                }

            });

        if(readoption) {
            await ReadAsync(lines);
        }
        else if(storeoption) {
            SaveCheep(cheepMessage);

        }

    }

    public static async Task ReadAsync(int? limit = 10)
    {
        // port: 5248
        var baseURL = "http://localhost:5248";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);
        List<Cheep> records = await client.GetFromJsonAsync<List<Cheep>>("Cheeps");

        var cheeps = new List<Cheep>();
        

        foreach (var record in records)
        {
            cheeps.Add(new Cheep(record.Author, record.Message, record.Timestamp));
        }

        UserInterface.PrintCheeps(cheeps);



    }

    public static async Task SaveCheepAsync(string message)
    {
        string author = Environment.UserName; //Takes username from computer
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        Console.WriteLine(author + ",\"" + message + "\"," + timestamp); //For debugging

        var baseURL = "http://localhost:5248";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);
        var db = SimpleDB.ChirpDB.Instance;
        var cheep = new SimpleDB.Cheep { Id = author, Message = message, Time = timestamp };
        var response = await client.PostAsJsonAsync("Cheep", cheep);
        Console.WriteLine(author + ",\"" + message + "\"," + timestamp + "YAAAY!"); //For debugging

        //db.Store(cheep);
    }
}