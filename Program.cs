using System.Numerics;
using System.Text.RegularExpressions;
using CsvHelper;
using System.Collections;
using CommandLine;
using System.ComponentModel;
using CsvHelper.Configuration;
using System.Globalization;




public class Program
{

    
    public class Options
    {
        [Option("read", Group = "action", Required = false, HelpText = "Reads all cheeps")]
        public bool Read { get; set; }

        [Option("cheep", Group = "action", Required = false, HelpText = "To send a cheep, write: run --cheep \"<message>\" ")]
        public IEnumerable<string>? cheepMessage { get; set; }
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
                if (o.cheepMessage != null && o.cheepMessage.Count() > 0) 
                {
                    SaveCheep(o.cheepMessage);
                } else {
                    args = new[] { "--help"};
                }

            });
    }
    

    public static void Read()
    {


        using var reader = new StreamReader("chirp_cli_db.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Foo>();
        foreach (var record in records) 
        {
            Console.WriteLine($"{record.Id} @ {record.Name}: {record.Time}");
        } 
    }

    public static void SaveCheep(IEnumerable<string> message)
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

        var record = new List<Foo>
        {
            new Foo {Id = author, Name = cheepString, Time = timestamp}
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