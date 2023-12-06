namespace Chirp.Razor.Tests;
using Xunit;
using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Humanizer;

public class PageInfoTests
{
    ChirpDBContext context;
    SqliteConnection _connection;

    public PageInfoTests(ChirpDBContext dbContext)
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var _contextOptions = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection).Options;
        context = new ChirpDBContext(_contextOptions);
        context.Database.EnsureCreated();

    }
    [Fact]
    public void OnGetAuthorTest()
    {
        // Arrange


        // Act


        // Assert

    }

    [Fact]
    public void OnGetTest()
    {
        // Arrange


        // Act


        // Assert
    }

    [Fact]
    public void FollowingCountTest()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public void FollowersCountTest()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public async Task ForgetMeTest()
    {
        // Arrange - Create three authors, one following two others, and one following one other.
        AuthorRepository authorRepository = new AuthorRepository(context);
        CheepRepository cheepRepository = new CheepRepository(context);
        // Author 1
        string author1Name = "ArthurAuthor1";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author1Name, ""));
        // Author 2
        string author2Name = "BalderAuthor2";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author2Name, ""));
        // Author 3
        string author3ame = "CamillaAuthor3";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author3ame, ""));

        // Getting AuthorDTOs as we would in the normal flow of the application
        var author1 = await authorRepository.FindAuthorByName(author1Name);
        var author2 = await authorRepository.FindAuthorByName(author2Name);
        var author3 = await authorRepository.FindAuthorByName(author3ame);

        // In the application it is impossible for users to follow other users, that haven't yet been created as authors.
        // - You are created as an author when you create your first cheep, or the first time you attempt to follow an author.
        // Author 1 follows Author 2 and Author 3
        await authorRepository.FollowAuthor(author1, author2);
        await authorRepository.FollowAuthor(author1, author3);




        // Act

        // Assert
    }

}