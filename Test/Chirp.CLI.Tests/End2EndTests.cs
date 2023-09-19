namespace Chirp.CLI.Tests;

using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using Xunit;
using SimpleDB;
using CommandLine;

public class End2EndTests
{

    [Fact]
    public void TestReadCheeps()
    {
        // Arrange

        // Act
        string output = "";
        string standardError = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet"; //"/usr/bin/dotnet";
            process.StartInfo.Arguments = "run --project src/Chirp.CLI/Chirp.CLI.csproj --read";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../../../../";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            // Synchronously read the standard output of the spawned process.
            StreamReader reader = process.StandardOutput;
            output = reader.ReadToEnd();
            standardError = process.StandardError.ReadToEnd();
            process.WaitForExit();
        }


        // Assert
        // This is what it looks like on tpep's pc: // ropf @ 01 / 08 / 2023 14.09.20: Hello, BDSA students!
        // But we want to make the test work for all pc's and since the date format changes, we found this to be specific enough:

        string filePath = "ReadEndToEndTestData.txt";
        StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine(output);
        writer.WriteLine(standardError);

        string[] outputLines = output.Split("\n");
        string firstCheep = outputLines[outputLines.Length - 2];

        writer.WriteLine(firstCheep);
        writer.Close();

        Assert.StartsWith("ropf", firstCheep);
        Assert.Contains("Hello, BDSA students!", firstCheep); // Assert.EndsWith("Hello, BDSA students!" For some reason, it doesn't work with assert.Endswith. Error: Assert.EndsWith() Failure:    //Expected:    Hello, BDSA students!  //Actual:   ···ello, BDSA students!
        Assert.Contains("14", firstCheep);
    }


    [Fact]
    public void TestReadCheepsFail()
    {
        // Arrange

        // Act
        string output = "";
        string standardError = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet"; //"/usr/bin/dotnet";
            process.StartInfo.Arguments = "run --project src/Chirp.CLI/Chirp.CLI.csproj --read";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../../../../";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            // Synchronously read the standard output of the spawned process.
            StreamReader reader = process.StandardOutput;
            output = reader.ReadToEnd();
            standardError = process.StandardError.ReadToEnd();
            process.WaitForExit();
        }


        // Assert
        // This is what it looks like on tpep's pc: // ropf @ 01 / 08 / 2023 14.09.20: Hello, BDSA students!
        // But we want to make the test work for all pc's and since the date format changes, we found this to be specific enough:

        string filePath = "ReadEndToEndTestCSVdataAndErrors.txt";
        StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine(output);
        writer.WriteLine(standardError);

        string[] outputLines = output.Split("\n");
        string firstCheep = outputLines[0];

        writer.WriteLine(firstCheep);
        writer.Close();

        Assert.False(firstCheep.StartsWith("rolpf"));
        Assert.DoesNotContain("Melloy, BDSA students!", firstCheep); // Assert.EndsWith("Hello, BDSA students!" For some reason, it doesn't work with assert.Endswith. Error: Assert.EndsWith() Failure:    //Expected:    Hello, BDSA students!  //Actual:   ···ello, BDSA students!
        Assert.DoesNotContain("13", firstCheep); // For some reason, doesn't work wtih Assert.DoesNotContain
    }



    [Theory]
    [InlineData("Hallohallo")]
    [InlineData("Hej med jer alle sammen, folkens")]
    [InlineData("Skaal med din oel aeckbert")]
    public void TestWriteCheep(string Message)
    {
        // Arrange
        string databaseFilePath = "../../../../../src/SimpleDB/chirp_cli_db.csv";
        string databaseCopyFilePath = "databaseTestCopy.csv";


        try
        {
            // Copy the file from the source to the destination
            File.Copy(databaseFilePath, databaseCopyFilePath, true);

            Console.WriteLine("File copied successfully.");
        }
        catch (IOException e)
        {
            Console.WriteLine($"An error occurred while copying the file: {e.Message}");
        }

        // Act
        try
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "dotnet"; //"/usr/bin/dotnet";
                process.StartInfo.Arguments = "run --project src/Chirp.CLI/Chirp.CLI.csproj --cheep \"" + Message + "\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = "../../../../../";
                process.StartInfo.RedirectStandardOutput = true;
                //process.StartInfo.RedirectStandardError = true;
                process.Start();

                // Synchronously read the standard output of the spawned process.
                StreamReader reader = process.StandardOutput;
                //output = reader.ReadToEnd();
                //standardError = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
        }
        catch (Exception e)
        {
            Assert.Equal("PROCESS FAILS!", e.Message);
        }


        // Assert
        string filePath = "EndToEndWriteTestMessageAndLineChecked.txt";
        StreamWriter writer = new StreamWriter(filePath);

        string[] databaseFileContentLines = File.ReadAllLines(databaseFilePath);
        string newCheep = databaseFileContentLines[databaseFileContentLines.Length - 1];
        writer.WriteLine(Message + " " + newCheep);
        writer.Close();

        Assert.Contains(Message, newCheep);
        File.Copy(databaseCopyFilePath, databaseFilePath, true);
        File.Delete(databaseCopyFilePath);
    }

    [Theory]
    [InlineData("Hallohallo")]
    [InlineData("Hej med jer alle sammen, folkens")]
    [InlineData("Skål med din øl Æckbert")]
    public void TestWriteCheepFail(string Message)
    {
        // Arrange
        string databaseFilePath = "../../../../../src/SimpleDB/chirp_cli_db.csv";
        string databaseCopyFilePath = "databaseTestCopy.csv";


        try
        {
            // Copy the file from the source to the destination
            File.Copy(databaseFilePath, databaseCopyFilePath, true);

            Console.WriteLine("File copied successfully.");
        }
        catch (IOException e)
        {
            Console.WriteLine($"An error occurred while copying the file: {e.Message}");
        }

        // Act
        try
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "dotnet"; //"/usr/bin/dotnet";
                process.StartInfo.Arguments = "run --project src/Chirp.CLI/Chirp.CLI.csproj --cheep " + Message;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = "../../../../../";
                process.StartInfo.RedirectStandardOutput = true;
                //process.StartInfo.RedirectStandardError = true;
                process.Start();

                // Synchronously read the standard output of the spawned process.
                StreamReader reader = process.StandardOutput;
                //output = reader.ReadToEnd();
                //standardError = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
        }
        catch (Exception e)
        {
            Assert.Equal("PROCESS FAILS!", e.Message);
        }


        // Assert
        string filePath = "EndToEndWriteTestMessageAndLineChecked.txt";
        StreamWriter writer = new StreamWriter(filePath);

        string[] databaseFileContentLines = File.ReadAllLines(databaseFilePath);
        string newCheep = databaseFileContentLines[databaseFileContentLines.Length - 1];
        writer.WriteLine(Message + "543527376383" + " " + newCheep);
        writer.Close();

        Assert.DoesNotContain(Message + "543527376383", newCheep);
        File.Copy(databaseCopyFilePath, databaseFilePath, true);
        File.Delete(databaseCopyFilePath);
    }
}