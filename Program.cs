﻿using System.Numerics;
using System.Text.RegularExpressions;
using System.Collections;

static void Main(string[] args){
if (args[0] == "read")
{
    var lines = File.ReadLines("chirp_cli_db.csv");
    foreach (var line in lines)
    {
        //Should format and prints all cheeps, but splits incorrectly i.e. in the cheep itself
        Regex regex = new Regex("(?<username>.+?),\"(?<message>.+)\",(?<time>[0-9]{10})");

        Match match = regex.Match(line);

        if (match.Success)
        {
        string author = match.Groups["username"].Value;
        string message = match.Groups["message"].Value;
        string timestamp = match.Groups["time"].Value;
        // Converts unix time to DateTime
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(long.Parse(timestamp)).ToLocalTime();
        Console.WriteLine($"{author} @ {dateTime}: {message}");
        }
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
}