using System.Numerics;
using System.Text.RegularExpressions;
using System.Collections;

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
    StreamWriter sw = new StreamWriter("chirp_cli_db.csv", true); // Chech whether encodeing language needs to be specified.
    ArrayList cheepList = new ArrayList();
    string cheepString;
    string author;
    long timestamp;
    //Enables cheeps with spaces
    for (int i = 1; i < args.Length; i++)
    {
        cheepList.Add(args[i]);
    }
    //Takes username from computer
    author = Environment.UserName;
    Console.WriteLine(cheepList);
    cheepString = string.Join(" ", cheepList.ToArray());
    timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    Console.WriteLine(author + ",\"" + cheepString + "\"," + timestamp); // For testing
    sw.WriteLine("");
    sw.Write(author + ",\"" + cheepString + "\"," + timestamp);
    sw.Close();
}