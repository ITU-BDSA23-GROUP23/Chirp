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
            .WithParsed<Options>(o =>
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

        if (readoption)
        {
            await ReadAsync(lines);
        }
        else if (storeoption)
        {
            using HttpClient client = new();
            await SaveCheepAsync(cheepMessage, client);

        }

    }

    public static async Task ReadAsync(int? limit = 10)
    {
        // port: 5248
        var baseURL = "https://bdsagroup23chirpremotedb.azurewebsites.net/";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        var cheeps = await client.GetFromJsonAsync<List<Cheep>>("Cheeps");

        foreach (var cheep in cheeps)
        {
            UserInterface.PrintCheeps(cheeps);
        }
    }

    public static async Task SaveCheepAsync(string message, HttpClient httpClient)
    {
        string author = Environment.UserName;
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var baseURL = "https://bdsagroup23chirpremotedb.azurewebsites.net/";
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.BaseAddress = new Uri(baseURL);

        var cheep = new Cheep(author, message, timestamp);

        var response = await httpClient.PostAsJsonAsync("Cheep", cheep);
        response.EnsureSuccessStatusCode();

    }
}