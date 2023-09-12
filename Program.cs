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

    public static void SaveCheep(IEnumerable<string> message)
    {
        string author = Environment.UserName; //Takes username from computer
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string cheepString;
        
        //Enables cheeps without ""
        ArrayList cheepList = new ArrayList();
        foreach (String word in message)
        {
            cheepList.Add(word);
        }
        cheepString = string.Join(" ", cheepList.ToArray());

        Console.WriteLine(author + ",\"" + cheepString + "\"," + timestamp); // For testing

        StreamWriter sw = new StreamWriter("chirp_cli_db.csv", true); // Chech whether encodeing language needs to be specified.
        //write new cheep to csv
        sw.WriteLine("");
        sw.Write(author + ",\"" + cheepString + "\"," + timestamp);

        sw.Close();
    }
}