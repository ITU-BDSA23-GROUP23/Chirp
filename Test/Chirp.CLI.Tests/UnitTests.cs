namespace Chirp.CLI.Tests;


public class UnitTests
{
    Program program;
    UserInterface userInterface;
    [Fact]
    public void IsDataStoredCorrectTest()
    {
        // Arrange
        IEnumerable<string> message = new List<string> { "hej" };
        var program = new Program();

        // Create a mock database for testing purposes
        var dbPath = "test_db.csv";
        var mockDb = new ChirpDB(dbPath);

        // Act
        program.SaveCheep(message, mockDb);

        // Assert
        var savedCheep = "";
        Assert.Equal("hej", savedCheep);

        // Clean up the test database file
        File.Delete(dbPath);
    }
    [Fact]
    public void TestTimestampConversion()
    {
        // Arrange
        long unixTimestamp = 1631712052 //This is for examples
        string expectedFormattedTime = "2021-09-16 09:20:52";
        userInterface = new UserInterface();
        /*long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); //Our method for unixTimestamp in SaveCheep() in Program
        string expectedFormattedTime = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"); */

        // Act
        var formattedTime = userInterface.ConvertFromUnixTime(unixTimestamp);

        // Assert
        Assert.Equal(expectedFormattedTime, formattedTime);
    }
}