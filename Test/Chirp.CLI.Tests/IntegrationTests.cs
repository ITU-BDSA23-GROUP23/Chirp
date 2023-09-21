using System.Collections;
using System;
using System.IO;

namespace Chirp.CLI.Tests;

/* // SEE BOTTOM OF ENDTOENDTESTS.cs!!!!!!!!!!!!
[CollectionDefinition("Filetampering")]

public class IntegrationTests
{

    [Fact]
    public void WriteReadIntegrationTest()
    {
        //Arrange
        String originalWorkingDirectory = Environment.CurrentDirectory;
        string simulatedDirectory = Path.Combine(originalWorkingDirectory, "../../../../../src/SimpleDB"); ;
        Directory.SetCurrentDirectory(simulatedDirectory);



        StringWriter sw = new StringWriter();
        Console.SetOut(sw);
        string message = "hejtest";

        //Act

        try
        {
            Program.SaveCheep(message);
            Program.Read();
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
        sw.Close();
        Directory.SetCurrentDirectory(originalWorkingDirectory);

    }
}
*/