using System.Collections;
using System;
using System.IO;
using Moq;

namespace Chirp.CLI.Tests;

// SEE BOTTOM OF ENDTOENDTESTS.cs!!!!!!!!!!!!
[Collection("Filetampering")]

public class IntegrationTests
{

    [Fact]
    public async Task WriteReadIntegrationTest()
    {

        // Arrange
        string databaseFilePath = "../../../../../src/Chirp.CSVDBService/chirp_cli_db.csv";
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

        string originalWorkingDirectory = Environment.CurrentDirectory;

        try
        {
            string simulatedDirectory = Path.Combine(originalWorkingDirectory, "../../../../../src/Chirp.CSVDBService");
            Directory.SetCurrentDirectory(simulatedDirectory);



            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            string message = "hejtest";

            //Act

            try
            {
                using HttpClient client = new();
                await Program.SaveCheepAsync("hejtest", client);
                await Program.ReadAsync();
            }
            catch (Exception e)
            {
                Assert.Equal("Act failed!", e.Message);
            }



            //Assert
            string expectedOutput = "hejtest";
            //"Ord 1 Ord 2 Ord 3";
            string[] outputLines = sw.ToString().Trim().Split("\n");
            string lastLine = outputLines[0];
            Assert.Contains(expectedOutput, lastLine);
        }
        catch (Exception e)
        { Assert.Equal("Act failed!", e.Message); }
        finally
        {
            Directory.SetCurrentDirectory(originalWorkingDirectory);
            File.Copy(databaseCopyFilePath, databaseFilePath, true);
            File.Delete(databaseCopyFilePath);
        }
    }
}