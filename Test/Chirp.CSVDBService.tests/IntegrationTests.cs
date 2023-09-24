namespace Chirp.CSVDBService.tests;
using Chirp.CLI;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;


public class IntegrationTests
{
    [Fact]
    public async Task HTTPGetCheepsTest()
    {
        // Arrange
        string author = "TestAuthor"; //Takes username from computer
        string message = "TestMessage";
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var baseURL = "http://localhost:5248";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        // Act
        var cheep = new Cheep(author, message, timestamp);
        //var response = await client.PostAsJsonAsync("Cheep", cheep);
        var response = await client.GetAsync("Cheeps");
        //response.HttpResponseMessage.Equals(200);
        var cheepList = await response.Content.ReadFromJsonAsync<List<Cheep>>();

        // Assert
        Assert.True(cheepList is List<Cheep>);
        Assert.Equal(200, (int)response.StatusCode);
    }

    [Fact]
    public async Task HTTPGetCheepsTestFail()
    {
        // Arrange
        string author = "TestAuthor"; //Takes username from computer
        string message = "TestMessage";
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var baseURL = "http://localhost:5248";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        // Act
        var cheep = new Cheep(author, message, timestamp);
        //var response = await client.PostAsJsonAsync("Cheep", cheep);
        var response = await client.GetAsync("Cheeps");
        //response.HttpResponseMessage.Equals(200);
        var cheepList = await response.Content.ReadFromJsonAsync<List<Cheep>>();

        // Assert
        Assert.False(cheepList is List<String>);
        Assert.NotEqual(201, (int)response.StatusCode);
    }



    [Fact]
    public async Task HTTPPostCheepTest()
    {
        // Arrange
        string author = "TestAuthor"; //Takes username from computer
        string message = "TestMessage";
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var baseURL = "http://localhost:5248";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        // Act
        var cheep = new Cheep(author, message, timestamp);
        var response = await client.PostAsJsonAsync("Cheep", cheep);

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }

    [Fact]
    public async Task HTTPPostCheepTestFail()
    {
        // Arrange
        string author = "TestAuthor"; //Takes username from computer
        string message = "TestMessage";
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var baseURL = "http://localhost:5248";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        // Act
        var cheep = new Cheep(author, message, timestamp);
        var response = await client.PostAsJsonAsync("Cheep", cheep);

        // Assert
        Assert.NotEqual(201, (int)response.StatusCode);
    }
}