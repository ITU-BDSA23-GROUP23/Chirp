using System;
using System.Collections.Generic;

public static class UserInterface
{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            Console.WriteLine($"{cheep.Author} @ {cheep.Timestamp}: {cheep.Message}");
        }
    }
}