using System.Collections;

namespace Chirp.CLI.Tests;

public class IntegrationTests
{
    Program program;

    [Fact]
    public void ReadWriteIntegrationTest()
    {
        //Arrange
        IEnumerable<string> message = new List<string> { "Ord 1", "Ord 2", "Ord 3" };
        program = new Program();

        //Act
        program.SaveCheep(message);

        //Assert
        Assert.
    }
}