namespace Chirp.Razor.Tests;
using Xunit;
using System.Net;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

public class AuthorRepositoryTests : IDisposable
{
    ChirpDBContext context;
    SqliteConnection _connection;

    public AuthorRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var _contextOptions = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection).Options;
        context = new ChirpDBContext(_contextOptions);
        context.Database.EnsureCreated();
    }

    [Fact]
    public void CreateAuthorTest()
    {
        //Arrange
        CreateAuthorDTO authorDTO = new("Thorbjørnen", "tpep@bjørn.dk");
        IAuthorRepository authorRepository = new AuthorRepository(context);
        //Act
        authorRepository.CreateAuthor(authorDTO);

        //Assert
        var addedAuthor = context.Authors.SingleOrDefault(a => a.Name == authorDTO.Name && a.Email == authorDTO.Email);
        Assert.Equal("Thorbjørnen", addedAuthor.Name);
        Assert.Equal("tpep@bjørn.dk", addedAuthor.Email);
    }

    [Fact]
    public void CreateAuthorEmailFailTest()
    {
        //Arrange
        CreateAuthorDTO authorDTO = new("Thorbjørnen", "tpepbjørn.dk");
        IAuthorRepository authorRepository = new AuthorRepository(context);
        //Act
        authorRepository.CreateAuthor(authorDTO);

        //Assert
        var addedAuthor = context.Authors.SingleOrDefault(a => a.Name == authorDTO.Name && a.Email == authorDTO.Email);
        Assert.Equal("Thorbjørnen", addedAuthor.Name);
        Assert.Equal("tpepbjørn.dk", addedAuthor.Email);
    }


    [Fact]
    public async Task FindAuthorByEmail()
    {
        //Arrange
        CreateAuthorDTO authorDTO = new("Thorbjørnen1", "tpep1@bjørn.dk");
        IAuthorRepository authorRepository = new AuthorRepository(context);
        authorRepository.CreateAuthor(authorDTO);

        //Act
        AuthorDTO? foundAuthor = await authorRepository.FindAuthorByName(authorDTO.Name);

        //Assert
        Assert.NotNull(foundAuthor);
        Assert.True(foundAuthor.Email == "tpep1@bjørn.dk");
    }

    [Fact]
    public async Task FindAuthorByName()
    {
        //Arrange
        CreateAuthorDTO authorDTO = new("Thorbjørnen2", "tpep2@bjørn.dk");
        IAuthorRepository authorRepository = new AuthorRepository(context);
        authorRepository.CreateAuthor(authorDTO);

        //Act
        AuthorDTO? foundAuthor = await authorRepository.FindAuthorByName(authorDTO.Name);

        //Assert
        Assert.NotNull(foundAuthor);
        Assert.True(foundAuthor.Name == "Thorbjørnen2");
    }

    [Fact]
    //make a followauthor test
    public async Task FollowAuthorTest()
    {
        // Arrange
        IAuthorRepository authorRepository = new AuthorRepository(context);
        string author1Name = "ArthurAuthor1";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author1Name, ""));
        // Author 2
        string author2Name = "BalderAuthor2";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author2Name, ""));

        var author1 = await authorRepository.FindAuthorByName(author1Name);
        var author2 = await authorRepository.FindAuthorByName(author2Name);

        // Act
        await authorRepository.FollowAuthor(author1, author2);

        // Assert
        Assert.True(authorRepository.IsFollowing(author1Name, author2Name));
    }
    [Fact]
    public async Task UnfollowAuthorTest()
    {
        // Arrange
        IAuthorRepository authorRepository = new AuthorRepository(context);
        string author1Name = "ArthurAuthor1";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author1Name, ""));
        // Author 2
        string author2Name = "BalderAuthor2";
        authorRepository.CreateAuthor(new CreateAuthorDTO(author2Name, ""));

        var author1 = await authorRepository.FindAuthorByName(author1Name);
        var author2 = await authorRepository.FindAuthorByName(author2Name);

        // Act
        await authorRepository.FollowAuthor(author1, author2);
        await authorRepository.UnfollowAuthor(author1, author2);

        // Assert
        Assert.False(authorRepository.IsFollowing(author1Name, author2Name));
    }

    public void Dispose()
    {
        context.Dispose();
        _connection.Dispose();
    }

}
