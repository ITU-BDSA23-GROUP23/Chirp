namespace Chirp.CLI.Tests;
using SimpleDB;


public class UnitTests
{
    [Fact]
    public void IsDataStoredCorrectTest()
    {
        // Arrange
        IEnumerable<string> message = new List<string> { "test" };
        var dbPath = "test_db.csv";
        var mockDb = SimpleDB.ChirpDB.Instance; // Creating a mock database to test

        // Act
        Program.SaveCheep(message);

        // Assert
        var savedCheep = mockDb.Read();
        Assert.Single(savedCheep); // We assume we only have 1 cheep for this unit test

        Assert.Equal(message.First(), savedCheep.First().Name); // Extract the message from the saved cheep and compare

        File.Delete(dbPath);  // Clean up the test database file
    }

    [Fact]
    public void TestTimestampConversion()
    {
        // Arrange
        long unixTimestamp = 1631712052;
        string expectedFormattedTime = "2021-09-16 09:20:52";
        /*long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); //Our method for unixTimestamp in SaveCheep() in Program
        string expectedFormattedTime = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"); */

        // Act
        DateTime formattedTime = UserInterface.ConvertFromUnixTime(unixTimestamp);
        string formattedTimeAsString = formattedTime.ToString("yyyy-MM-dd HH:mm:ss");

        // Assert
        Assert.Equal(expectedFormattedTime, formattedTimeAsString);
    }
}