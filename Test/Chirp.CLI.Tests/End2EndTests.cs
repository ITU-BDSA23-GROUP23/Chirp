namespace Chirp.CLI.Tests;

public class End2EndTests
{
    [Fact]
    public void ReadEndToEndTest()
    {
        StringWriter sw = new StringWriter();
        Console.SetOut(sw);
        //Arrange
        IEnumerable<string> message = new List<string> { "hejtest" };

        //Act

        Program.SaveCheep(message);
        Program.Read();

        //Assert
        string expectedOutput = "hejtest";
        //"Ord 1 Ord 2 Ord 3";
        string[] outputLines = sw.ToString().Trim().Split(Environment.NewLine);
        string lastLine = outputLines[outputLines.Length - 1];
        Assert.Contains(expectedOutput, lastLine);
    }
}