using System.Numerics;
using System.Text.RegularExpressions;

if (args[0] == "read")
{
    var lines = File.ReadLines("chirp_cli_db.csv");
    foreach (var line in lines)
    {
        //Should format and prints all cheeps, but splits incorrectly i.e. in the cheep itself
        string[] toRead = line.Split(",");
        string author = toRead[0];
        string message = toRead[1];
        string timestamp = toRead[2];
        Console.WriteLine($"{author} @ {timestamp}: {message}");

    }
}
else if (args[0] == "cheep")
{
    string cheep = "";
    //Enables cheeps with spaces
    for (int i = 1; i < args.Length; i++)
    {
        cheep += " " + args[i];
    }
    //Takes username from computer
    string author = Environment.UserName;
    
    string timestamp = ("" + DateTime.Now); //To-do: Format like csv file example
    Console.WriteLine(cheep + " " + author + " " + timestamp);
}