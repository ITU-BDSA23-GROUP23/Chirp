namespace Chirp.Razor.Tests;
using Xunit;
using Microsoft.Data.Sqlite;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Chirp.Core;
using System.Data.Common;
using Chirp.Web.data;
public class CreateCheepTest
{
    ChirpDBContext context;
    SqliteConnection _connection;
    public CreateCheepTest()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var _contextOptions = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection).Options;
        context = new ChirpDBContext(_contextOptions);
        context.Database.EnsureCreated();
    }

    [Fact]
    public async Task CreateCheep()
    {
        //Arrange
        string Message = "MyMessage";
        ICheepRepository cheepRepository = new CheepRepository(context);
        IAuthorRepository authorRepository = new AuthorRepository(context);
        AuthorDTO author = new AuthorDTO("Thorstein Pedersen", "tpepsi@gmail.com");

        authorRepository.CreateAuthor(author);

        //Act
        cheepRepository.CreateCheep(author, Message);

        //Assert
        var cheeps = await cheepRepository.GetCheeps();
        foreach (var cheepDTO in cheeps)
        {
            Assert.Equal("Thorstein Pedersen", cheepDTO.AuthorName);
            Assert.Equal("MyMessage", cheepDTO.Message);
        }

    }

}