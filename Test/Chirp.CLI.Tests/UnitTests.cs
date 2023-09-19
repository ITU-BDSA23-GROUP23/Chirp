namespace Chirp.CLI.Tests;

using System.Runtime.CompilerServices;
using CsvHelper.Configuration.Attributes;
using SimpleDB;


public class UnitTests
{
    [Fact(Skip = "db file is not accessable for now")]
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


    //[Fact(Skip = "Skipped due to timezone differences")]
    [Fact]
    public void TestTimestampConversion()
    {
        // Arrange
        long unixTimestamp = 1631712052;
        string expectedFormattedTime = "2021-09-15 15:20:52";
        /*long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); //Our method for unixTimestamp in SaveCheep() in Program
        string expectedFormattedTime = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"); */

        // Act
        DateTime formattedTime = UserInterface.ConvertFromUnixTime(unixTimestamp);
        string formattedTimeAsString = formattedTime.ToString("yyyy-MM-dd HH:mm:ss");

        // Assert
        Assert.Equal(expectedFormattedTime, formattedTimeAsString);
    }
}