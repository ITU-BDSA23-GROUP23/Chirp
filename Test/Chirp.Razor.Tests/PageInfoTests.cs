namespace Chirp.Razor.Tests;
using Xunit;
using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Humanizer;
using Azure;
using System.Reflection;

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
        var author1cheep1DTO = new createCheepDTO(author1DTO, "Author 1 cheep 1");
        await cheepRepository.CreateCheep(author1cheep1DTO, null);
        var author1cheep2DTO = new createCheepDTO(author1DTO, "Author 1 cheep 2");
        await cheepRepository.CreateCheep(author1cheep2DTO, null);

        var author2cheep1DTO = new createCheepDTO(author2DTO, "Author 2 cheep 1");


        await authorRepository.FollowAuthor(author1DTO, author2DTO);
        await authorRepository.FollowAuthor(author1DTO, author3DTO);

        // We need to get the cheepIDs of the cheeps we just created, so we can react to them.
        var author1cheeps = await cheepRepository.GetCheeps(1, authorName: author1Name);
        foreach (var cheep in author1cheeps)
        {
            await cheepRepository.ReactToCheep(author2Name, "Like", cheep.Id);
            await cheepRepository.ReactToCheep(author2Name, "Like", cheep.Id);
            await cheepRepository.ReactToCheep(author3Name, "Love", cheep.Id);
            await cheepRepository.ReactToCheep(author3Name, "Love", cheep.Id);
        }
        var author2cheeps = await cheepRepository.GetCheeps(1, authorName: author1Name);
        foreach (var cheep in author2cheeps)
        {
            await cheepRepository.ReactToCheep(author1Name, "Like", cheep.Id);
            await cheepRepository.ReactToCheep(author1Name, "Love", cheep.Id);
        }


        // ACT Use forget me on author 1
        await authorRepository.ForgetMe(author1Name);


        // ASSERT
        // Check that author 2 and 3 no longer follows author 1, and that author 1 is completely removed from the database.
        var author1FollowERSAfter = await authorRepository.GetFollowers(author1Name);
        var author2FollowERSAfter = await authorRepository.GetFollowers(author2Name);
        var author3FollowERSAfter = await authorRepository.GetFollowers(author3Name);

        var author1FollowINGAfter = await authorRepository.GetFollowing(author1Name);
        var author2FollowINGAfter = await authorRepository.GetFollowing(author2Name);
        var author3FollowINGAfter = await authorRepository.GetFollowing(author3Name);

        // Check that author 2 and author 3 is no longer following or followers of author 1.
        Assert.Null(author1FollowERSAfter);
        Assert.Null(author1FollowINGAfter);

        // Now i want to go through author2FollowERSAfter and author3FollowERSAfter and check that they are not following author 1.
        // Check that there is no list left of author 1 following author 1 and author 2.

        // author2 lists checks:
        foreach (var author in author2FollowERSAfter)
        {
            Assert.NotEqual(author1Name, author.Name);
        }
        foreach (var author in author2FollowINGAfter)
        {
            Assert.NotEqual(author1Name, author.Name);
        }

        // Author3 lists checks:
        foreach (var author in author3FollowERSAfter)
        {
            Assert.NotEqual(author1Name, author.Name);
        }
        foreach (var author in author2FollowINGAfter)
        {
            Assert.NotEqual(author1Name, author.Name);
        }

        // Check that all reactions to author 1 cheeps are removed.
        // Go through all the cheeps in author1cheeps and check that all reactions to author 1 cheeps are removed.
        foreach (var cheep in author1cheeps)
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await cheepRepository.GetReactions(cheep.Id, 1));
            // If the exception is thrown (passing the assertion), the reactions with these ids have removed.
        }

        // Check that all reactions by author 1 are removed.
        var author1Reactions = await context.Reactions.FirstOrDefaultAsync(r => r.Author.Name == author1Name);
        Assert.Null(author1Reactions);

        // Check that author 1 is completely removed from the database.
        var author1After = await authorRepository.FindAuthorByName(author1Name);
        Assert.Null(author1After);
    }

    public void Dispose()
    {
        context.Dispose();
        _connection.Dispose();
    }
}