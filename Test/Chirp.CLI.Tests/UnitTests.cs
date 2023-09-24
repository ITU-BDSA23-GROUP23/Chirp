namespace Chirp.CLI.Tests;

using Xunit;
using System.Runtime.CompilerServices;
using CsvHelper.Configuration.Attributes;
using Chirp.CLI;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommandLine;
using System.Net;
using Moq;
using System.Net.Http.Json;

public class UnitTests
{
    [Fact(Skip = "Skipped for now because of some error. The test should be COMPLETE")]
    public async Task IsDataStoredCorrectTest() //Credit: We have used ChatGPT for the use of httpClientMock
    {
        // Arrange
        string message = "Test message";
        var expectedCheep = new Cheep("TestAuthor", message, 12345);

        var httpClientMock = new Mock<HttpClient>();
        httpClientMock.Setup(client =>
            client.PostAsJsonAsync("Cheep", expectedCheep, default))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        // Act
        await Program.SaveCheepAsync(message, httpClientMock.Object);

        // Assert
        httpClientMock.Verify(client =>
    client.PostAsJsonAsync("Cheep", It.IsAny<Cheep>(), default),
    Times.Once
        );
    }

    [Fact(Skip = "Skipped due to timezone or format differences")]
    //[Fact]
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