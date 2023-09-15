namespace Chirp.CLI.Tests;


public class UnitTests
{
    Program program;
    UserInterface userInterface;

    [Fact]
    public void IsDataStoredCorrectTest()
    {
        // Arrange
        IEnumerable<string> message = new List<string> { "test" };
        var program = new Program();

        var dbPath = "test_db.csv";
        var mockDb = new ChirpDB(dbPath); // Creating a mock database to test

        // Act
        program.SaveCheep(message, mockDb);

        // Assert
        var savedCheep = mockDb.Read();
        Assert.Single(savedCheep); //This is while we assume we only have 1 cheep for this unit test
        Assert.Equal(message, savedCheep[0].Name); //we say .Name because we want to compare first Cheep with msg

        File.Delete(dbPath);  // Clean up the test database file
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