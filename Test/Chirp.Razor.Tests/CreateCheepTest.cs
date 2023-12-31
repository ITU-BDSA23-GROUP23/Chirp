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
        IAuthorRepository authorRepository = new AuthorRepository(context);
        ICheepRepository cheepRepository = new CheepRepository(context, authorRepository);
        CreateAuthorDTO author = new CreateAuthorDTO("Thorstein Pedersen", "tpepsi@gmail.com");
        authorRepository.CreateAuthor(author);
        var Aurthor = authorRepository.FindAuthorByName("Thorstein Pedersen").Result;
        createCheepDTO createcheepDTO = new createCheepDTO(Aurthor, Message);
       

        //Act
        cheepRepository.CreateCheep(createcheepDTO, null);

        //Assert
        var cheeps = await cheepRepository.GetCheeps();
        foreach (var cheepDTO in cheeps)
        {
            Assert.Equal("Thorstein Pedersen", cheepDTO.AuthorName);
            Assert.Equal("MyMessage", cheepDTO.Message);
        }

    }

}