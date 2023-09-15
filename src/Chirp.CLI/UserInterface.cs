namespace Chirp.CLI;
using System;
using System.Collections.Generic;
using System.Numerics;

public static class UserInterface
{

    public static DateTime ConvertFromUnixTime(long Unixtime) {

        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(Unixtime).ToLocalTime();

        return dateTime;


    } 


    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
          
            Console.WriteLine($"{cheep.Author} @ {ConvertFromUnixTime(cheep.Timestamp)}: {cheep.Message}");
        }
    }
}