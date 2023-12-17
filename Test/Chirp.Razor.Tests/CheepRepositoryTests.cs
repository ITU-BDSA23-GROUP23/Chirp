namespace Chirp.Razor.Tests;
using Xunit;
using Microsoft.Data.Sqlite;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Chirp.Core;
using System.Data.Common;
using Chirp.Web.data;

public class CheepRepositoryTests : IDisposable
{
    ChirpDBContext context;
    SqliteConnection _connection;
    public CheepRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var _contextOptions = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection).Options;
        context = new ChirpDBContext(_contextOptions);
        context.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetCheepsTest()
    {
        IAuthorRepository authorRepository = new AuthorRepository(context);
        ICheepRepository cheepRepository = new CheepRepository(context, authorRepository);
        CreateAuthorDTO author = new CreateAuthorDTO("Jacqualine Gilcoine", "Jacqualine.Gilcoine@gmail.com");
        authorRepository.CreateAuthor(author);
        var Aurthor = authorRepository.FindAuthorByName("Jacqualine Gilcoine").Result;
        createCheepDTO createcheepDTO = new createCheepDTO(Aurthor, "123Testing");
        cheepRepository.CreateCheep(createcheepDTO, null);
        var cheeps = await cheepRepository.GetCheeps();
        foreach (CheepDTO _cheep in cheeps)
        {
            Assert.Equal(_cheep.Message, "123Testing");
        }
    }

    [Fact]
    public async Task GetCheepsAmountTest()
    {
        DbInitializer.Initialize(context);
        var authorRepository = new AuthorRepository(context);
        ICheepRepository cheepRepository = new CheepRepository(context, authorRepository);
        var cheeps = await cheepRepository.GetCheepsAmount("Mellie Yost");
        var authorCheeps = await authorRepository.GetCheepAmount("Mellie Yost");

        Assert.Equal(cheeps, 7);
        Assert.Equal(authorCheeps, 7);
    }


    public void Dispose()
    {
        context.Dispose();
        _connection.Dispose();
    }
}
