using System.Collections;
using System;
using System.IO;

namespace Chirp.CLI.Tests;

public class IntegrationTests
{

    [Fact]
    public void WriteReadIntegrationTest()
    {
        StringWriter sw = new StringWriter();
        Console.SetOut(sw);
        //Arrange
        IEnumerable<string> message = new List<string> { "hej" };

        //Act

        Program.SaveCheep(message);
        Program.Read();

        //Assert
        string expectedOutput = "hej";
        //"Ord 1 Ord 2 Ord 3";
        string[] outputLines = sw.ToString().Trim().Split(Environment.NewLine);
        string lastLine = outputLines[outputLines.Length - 1];
        Assert.Contains(expectedOutput, lastLine);
    }
}