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
        cheepString = string.Join(" ", cheepList.ToArray());
        foreach (String word in message)
        {
            cheepList.Add(word);
        }
        var record = new List<Foo>
        {
            new Foo {Id = author, Name = cheepString, Time = timestamp}
        };
        using (var writer = new StreamWriter("chirp_cli_db.csv"));
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(record);
        }
        

        //Enables cheeps without ""
        
        

        Console.WriteLine(author + ",\"" + cheepString + "\"," + timestamp); // For testing

        StreamWriter sw = new StreamWriter("chirp_cli_db.csv", true); // Chech whether encodeing language needs to be specified.
        //write new cheep to csv
        sw.WriteLine("");
        sw.Write(author + ",\"" + cheepString + "\"," + timestamp);

        sw.Close();
    }
}