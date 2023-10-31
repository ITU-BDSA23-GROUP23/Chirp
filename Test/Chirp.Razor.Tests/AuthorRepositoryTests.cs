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
        AuthorDTO authorDTO = new("Thorbjørnen", "tpep@bjørn.dk");
        AuthorRepository authorRepository = new(context);
        //Act
        authorRepository.CreateAuthor(authorDTO);

        //Assert
        var addedAuthor = context.Authors.SingleOrDefault(a => a.Name == authorDTO.Name && a.Email == authorDTO.Email);
        Assert.NotNull(addedAuthor);
    }



    [Fact]
    public void FindAuthorByEmail()
    {
        //Arrange
        AuthorDTO authorDTO = new("Thorbjørnen1", "tpep1@bjørn.dk");
        AuthorRepository authorRepository = new(context);
        authorRepository.CreateAuthor(authorDTO);

        //Act
        _ = authorRepository.FindAuthorByEmail(authorDTO.Email);

        //Assert
        var addedAuthor = context.Authors.SingleOrDefault(a => a.Name == authorDTO.Name && a.Email == authorDTO.Email);
        Assert.NotNull(addedAuthor);
        Assert.True(addedAuthor.Email == "tpep1@bjørn.dk");
    }

    [Fact]
    public async void FindAuthorByName()
    {
        //Arrange
        AuthorDTO authorDTO = new("Thorbjørnen2", "tpep2@bjørn.dk");
        AuthorRepository authorRepository = new(context);
        authorRepository.CreateAuthor(authorDTO);

        //Act
        AuthorDTO? foundAuthor = await authorRepository.FindAuthorByName(authorDTO.Name);

        //Assert
        //var addedAuthor = context.Authors.SingleOrDefault(a => a.Name == authorDTO.Name);
        Assert.NotNull(foundAuthor);
        Assert.True(foundAuthor.Name == "Thorbjørnen2");
    }

    public void Dispose()
    {
        context.Dispose();
        _connection.Dispose();
    }

}
