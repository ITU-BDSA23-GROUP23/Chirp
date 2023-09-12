using System.Numerics;
using System.Text.RegularExpressions;
using System.Collections;
using CommandLine;
using System.ComponentModel;

public class Program
{
    public class Options
    {
        [Option("read", Group = "action", Required = false, HelpText = "Reads all cheeps")]
        public bool Read { get; set; }

        [Option("cheep", Group = "action", Required = false, HelpText = "Save a cheep")]
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
                if (o.cheepMessage != null) //For some reason, it it is not null even though cheep isn't given as an option. This means that if someone runs the program without any of the legal options, the standard help message won't appear.
                {
                    SaveCheep(o.cheepMessage);
                }

            });
        /*
    if (args[0] == "read")
    {
        Read();
    }
    else if (args[0] == "cheep")
    {
        SaveCheep(args);
    }
    */
    }

    public static void Read()
    {
        var lines = File.ReadLines("chirp_cli_db.csv");
        int i = 0;
        Cheep[] cheeps = new Cheep[lines.Count()];
        foreach (var line in lines)
        {
            //Should format and prints all cheeps, but splits incorrectly i.e. in the cheep itself
            Regex regex = new Regex("(?<username>.+?),\"(?<message>.+)\",(?<time>[0-9]{10})");

            Match match = regex.Match(line);

            if (match.Success)
            {
                string author = match.Groups["username"].Value;
                string message = match.Groups["message"].Value;
                long timestamp = long.Parse(match.Groups["time"].Value);
                // Converts unix time to DateTime
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
                Console.WriteLine($"{author} @ {dateTime}: {message}");
                cheeps[i] = new Cheep(author, message, dateTime.Ticks);
            }
            i++;
        }
        UserInterface.PrintCheeps(cheeps);
    }

    public static void SaveCheep(IEnumerable<string> args)
    {
        StreamWriter sw = new StreamWriter("chirp_cli_db.csv", true); // Chech whether encodeing language needs to be specified.
        ArrayList cheepList = new ArrayList();
        string cheepString;
        string author;
        long timestamp;
        //Enables cheeps with spaces
        foreach (String arg in args)
        {
            cheepList.Add(arg);
        }
        //Takes username from computer
        author = Environment.UserName;
        //Console.WriteLine(cheepList);
        cheepString = string.Join(" ", cheepList.ToArray());
        timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        if (cheepString == "")
        {
            Console.WriteLine("to input a cheep, write: run --cheep \"<message>\" ");
        }
        else
        {
            Console.WriteLine(author + ",\"" + cheepString + "\"," + timestamp); // For testing
            sw.WriteLine("");
            sw.Write(author + ",\"" + cheepString + "\"," + timestamp);
        }
        sw.Close();
    }
}