namespace Chirp.CLI.Tests;

using System.Diagnostics;
using System.IO;
using Xunit;

public class End2EndTests
{
    [Fact]
    public void TestReadCheep()
    {
        // Arrange

        // Act
        string output = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet"; //"/usr/bin/dotnet";
            process.StartInfo.Arguments = "run --project src/Chirp.CLI/Chirp.CLI.csproj --read";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            // Synchronously read the standard output of the spawned process.
            StreamReader reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }
        string firstCheep = output.Split("\n")[0];
        // Assert
        // This is how what it looks like on tpep's pc: // ropf @ 01 / 08 / 2023 14.09.20: Hello, BDSA students!
        // But we want to make the test work for all pc's and since the date format changes, we found this to be rather thorough.

        Assert.StartsWith("ropf", firstCheep);
        Assert.EndsWith("Hello, BDSA students!", firstCheep);
        Assert.Contains("14", firstCheep);

    }
}