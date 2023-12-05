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
    // [Fact]
    // public void CreateCheepTest()
    // {

    // }

    [Fact]
    public async Task GetCheepsTest()
    {
        // var a10 = new Author() { Name = "Jacqualine Gilcoine", Email = "Jacqualine.Gilcoine@gmail.com", Cheeps = new List<Cheep>() };
        // var c1 = new Cheep() { Author = a10, Message = "123Testing", TimeStamp = DateTime.Parse("2023-08-01 13:14:37") };
        ICheepRepository cheepRepository = new CheepRepository(context);
        IAuthorRepository authorRepository = new AuthorRepository(context);
        CreateAuthorDTO author = new CreateAuthorDTO("Jacqualine Gilcoine", "Jacqualine.Gilcoine@gmail.com");
        authorRepository.CreateAuthor(author);
        var Aurthor = authorRepository.FindAuthorByName("Jacqualine Gilcoine").Result;
        createCheepDTO createcheepDTO = new createCheepDTO(Aurthor, "123Testing");
        cheepRepository.CreateCheep(createcheepDTO, null);
        // context.Authors.Add(a10);
        // context.Cheeps.Add(c1);
        // context.SaveChanges();
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
        ICheepRepository cheepRepository = new CheepRepository(context);
        var authorRepository = new AuthorRepository(context);

        var cheeps = await cheepRepository.GetCheepsAmount("Mellie Yost");
        var authorCheeps = await authorRepository.GetCheepAmount("Mellie Yost");

        Assert.Equal(cheeps, 7);
        Assert.Equal(authorCheeps, 7);
    }



    // [Fact]
    // public void CalculateSkippedCheepsTest()
    // {

    // }

    // [Fact]
    // public void CheepsToCheepDTOsTest()
    // {

    // }

    public void Dispose()
    {
        context.Dispose();
        _connection.Dispose();
    }
}
