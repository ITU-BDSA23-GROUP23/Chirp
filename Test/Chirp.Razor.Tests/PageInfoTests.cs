namespace Chirp.Razor.Tests;
using Xunit;
using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Humanizer;
using Azure;

public class PageInfoTests : IDisposable
{
    ChirpDBContext context;
    SqliteConnection _connection;

    IAuthorRepository authorRepository;
    ICheepRepository cheepRepository;

    public PageInfoTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var _contextOptions = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection).Options;
        context = new ChirpDBContext(_contextOptions);
        context.Database.EnsureCreated();
        authorRepository = new AuthorRepository(context);
        cheepRepository = new CheepRepository(context);
    }

    [Fact]
    public async Task GetAuthorTest()
    {
        // Arrange
        string authorName = "ArthurAuthor";
        authorRepository.CreateAuthor(new CreateAuthorDTO(authorName, ""));
        // Act
        var author = await authorRepository.FindAuthorByName(authorName);
        // Assert
        Assert.Equal(authorName, author.Name);
    }
    [Fact]
    public async Task GetFollowingTest()
    {
        // Arrange
        string author1Name = "ArthurAuthor1";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author1Name, ""));
        // Author 2
        string author2Name = "BalderAuthor2";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author2Name, ""));
        // Author 3
        string author3ame = "CamillaAuthor3";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author3ame, ""));

        var author1 = await authorRepository.FindAuthorByName(author1Name);
        var author2 = await authorRepository.FindAuthorByName(author2Name);
        var author3 = await authorRepository.FindAuthorByName(author3ame);

        await authorRepository.FollowAuthor(author2, author1);
        await authorRepository.FollowAuthor(author3, author1);

        // Act
        var author1Following = await authorRepository.GetFollowing(author1Name);

        // Assert
        Assert.Equal(2, author1Following.Count());
    }

    [Fact]
    public async Task GetFollowersTest()
    {
        // Arrange
        string author1Name = "ArthurAuthor1";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author1Name, ""));
        // Author 2
        string author2Name = "BalderAuthor2";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author2Name, ""));
        // Author 3
        string author3ame = "CamillaAuthor3";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author3ame, ""));

        var author1 = await authorRepository.FindAuthorByName(author1Name);
        var author2 = await authorRepository.FindAuthorByName(author2Name);
        var author3 = await authorRepository.FindAuthorByName(author3ame);

        await authorRepository.FollowAuthor(author1, author2);
        await authorRepository.FollowAuthor(author1, author3);

        // Act
        var author1Followers = await authorRepository.GetFollowers(author1Name);

        // Assert
        Assert.Equal(2, author1Followers.Count());
    }

    [Fact]
    public async Task FollowingCountTest()
    {
        // Arrange

        string author1Name = "ArthurAuthor1";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author1Name, ""));
        // Author 2
        string author2Name = "BalderAuthor2";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author2Name, ""));
        // Author 3
        string author3ame = "CamillaAuthor3";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author3ame, ""));

        var author1 = await authorRepository.FindAuthorByName(author1Name);
        var author2 = await authorRepository.FindAuthorByName(author2Name);
        var author3 = await authorRepository.FindAuthorByName(author3ame);

        await authorRepository.FollowAuthor(author2, author1);
        await authorRepository.FollowAuthor(author3, author1);
        await authorRepository.FollowAuthor(author1, author2);

        // Act
        var author1FollowingCount = authorRepository.GetFollowingCount(author1Name);

        // Assert
        Assert.Equal(2, author1FollowingCount);
    }

    [Fact]
    public async Task FollowersCountTest()
    {
        // Arrange
        string author1Name = "ArthurAuthor1";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author1Name, ""));
        // Author 2
        string author2Name = "BalderAuthor2";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author2Name, ""));
        // Author 3
        string author3ame = "CamillaAuthor3";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author3ame, ""));

        var author1 = await authorRepository.FindAuthorByName(author1Name);
        var author2 = await authorRepository.FindAuthorByName(author2Name);
        var author3 = await authorRepository.FindAuthorByName(author3ame);

        await authorRepository.FollowAuthor(author2, author1);
        await authorRepository.FollowAuthor(author3, author1);
        await authorRepository.FollowAuthor(author1, author2);

        // Act
        var author1FollowingCount = authorRepository.GetFollowersCount(author1Name);

        // Assert
        Assert.Equal(1, author1FollowingCount);
    }
    [Fact]
    public async Task DeleteAuthorTest()
    {
        // Arrange
        string author1Name = "ArthurAuthor1";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author1Name, ""));
        // Author 2
        string author2Name = "BalderAuthor2";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author2Name, ""));
        // Author 3
        string author3ame = "CamillaAuthor3";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author3ame, ""));

        var author1 = await authorRepository.FindAuthorByName(author1Name);
        var author2 = await authorRepository.FindAuthorByName(author2Name);
        var author3 = await authorRepository.FindAuthorByName(author3ame);

        await authorRepository.FollowAuthor(author2, author1);
        await authorRepository.FollowAuthor(author3, author1);
        await authorRepository.FollowAuthor(author1, author2);

        // Act
        await authorRepository.DeleteAuthor(author1Name);

        // Assert
        Assert.Null(await authorRepository.FindAuthorByName(author1Name));
        Assert.Equal(0, authorRepository.GetFollowersCount(author1Name));
        Assert.Equal(0, authorRepository.GetFollowingCount(author1Name));
    }

    [Fact]
    public async Task RemoveFollowersTest()
    {
        // Arrange
        string author1Name = "ArthurAuthor1";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author1Name, ""));
        // Author 2
        string author2Name = "BalderAuthor2";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author2Name, ""));
        // Author 3
        string author3ame = "CamillaAuthor3";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author3ame, ""));

        var author1 = await authorRepository.FindAuthorByName(author1Name);
        var author2 = await authorRepository.FindAuthorByName(author2Name);
        var author3 = await authorRepository.FindAuthorByName(author3ame);

        await authorRepository.FollowAuthor(author2, author1);
        await authorRepository.FollowAuthor(author3, author1);
        await authorRepository.FollowAuthor(author1, author2);

        // Act
        var author1Followers = await authorRepository.GetFollowers(author1Name);
        await authorRepository.RemoveFollowers(author1Followers, author1Name);

        // Assert
        Assert.Equal(0, authorRepository.GetFollowersCount(author1Name));
    }

    [Fact]
    public async Task RemoveFollowingTest()
    {
        // Arrange
        string author1Name = "ArthurAuthor1";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author1Name, ""));
        // Author 2
        string author2Name = "BalderAuthor2";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author2Name, ""));
        // Author 3
        string author3ame = "CamillaAuthor3";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author3ame, ""));

        var author1 = await authorRepository.FindAuthorByName(author1Name);
        var author2 = await authorRepository.FindAuthorByName(author2Name);
        var author3 = await authorRepository.FindAuthorByName(author3ame);

        await authorRepository.FollowAuthor(author2, author1);
        await authorRepository.FollowAuthor(author3, author1);
        await authorRepository.FollowAuthor(author1, author2);

        // Act
        var author1Following = await authorRepository.GetFollowing(author1Name);
        await authorRepository.RemoveFollowing(author1Following, author1Name);

        // Assert
        Assert.Equal(0, authorRepository.GetFollowingCount(author1Name));
    }

    //Integration test
    [Fact]
    public async Task ForgetMeTest()
    {
        // Arrange - Create three authors, with 3 cheeps each, each following each other.
        // Author 1
        string author1Name = "ArthurAuthor1";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author1Name, ""));
        // Author 2
        string author2Name = "BalderAuthor2";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author2Name, ""));
        // Author 3
        string author3Name = "CamillaAuthor3";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author3Name, ""));

        // Getting AuthorDTOs as we would in the normal flow of the application
        var author1DTO = await authorRepository.FindAuthorByName(author1Name);
        var author2DTO = await authorRepository.FindAuthorByName(author2Name);
        var author3DTO = await authorRepository.FindAuthorByName(author3Name);

        // In the application it is impossible for users to follow other users, that haven't yet been created as authors.*      // (Except if they interact with the application outside our UI)
        // - You are created as an author when you create your first cheep, or the first time you attempt to follow an author.
        // Author 1 follows Author 2 and Author 3
        await authorRepository.FollowAuthor(author1DTO, author2DTO);
        await authorRepository.FollowAuthor(author1DTO, author3DTO);


        // Act Use forget me on author 1
        await authorRepository.ForgetMe(author1Name);


        // Assert Check that author 2 and 3 no longer follows author 1, and that author 1 is completely removed from the database.
        var author1FollowersAfter = await authorRepository.GetFollowers(author1Name);
        var author1FollowingAfter = await authorRepository.GetFollowing(author1Name);

        var author2FollowingAfter = await authorRepository.GetFollowing(author2Name);
        var author3FollowingAfter = await authorRepository.GetFollowing(author3Name);

        // Check that author 2 and author 3 is no longer following author 1.


        // Check that there is no list left of author 1 following author 1 and author 3.


        // Check that author 1 is completely removed from the database.
        // If the method return ArgumentNullException, when the author isn't found: await Assert.ThrowsAsync<ArgumentNullException>(async () => await authorRepository.FindAuthorByName(author1Name));
        var author1After = await authorRepository.FindAuthorByName(author1Name);
        Assert.Null(author1After);
    }

    public void Dispose()
    {
        context.Dispose();
        _connection.Dispose();
    }
}