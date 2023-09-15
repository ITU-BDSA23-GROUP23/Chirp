using System.Collections;

namespace Chirp.CLI.Tests;


public class ReadWriteIntegrationTest
{
    [Fact]
    public void Test1()
    {
        //Arrange
        IEnumerable<string> message = new List<string> { "Ord1", "Ord 2", "Ord 3" };

        //Act


        //Assert
        Assert.Equal(1, 1);
    }
}