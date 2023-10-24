namespace Chirp.Razor.Tests;
using Xunit;
using Microsoft.Data.Sqlite;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Chirp.Core;
using System.Data.Common;

public class CheepRepositoryTests : IDisposable
{
    ChirpDBContext context;
    SqliteConnection _connection;
    public CheepRepositoryTests()
    {

        _connection =  new SqliteConnection("Filename=:memory:");
        _connection.Open();


        
    }
    // [Fact]
    // public void CreateCheepTest()
    // {
        
    // }

    [Fact]
    public async Task GetCheepsTest()
    {
        var _contextOptions = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection).Options;
        using var context = new ChirpDBContext(_contextOptions);

        if (context.Database.EnsureCreated())
        {
            var a10 = new Author() { Name = "Jacqualine Gilcoine", Email = "Jacqualine.Gilcoine@gmail.com", Cheeps = new List<Cheep>() };
            var c1 = new Cheep() { Author = a10, Message = "123Testing", TimeStamp = DateTime.Parse("2023-08-01 13:14:37") };
            context.Authors.Add(a10);
            context.Cheeps.Add(c1);
            context.SaveChanges();
            var cheepRepository = new CheepRepository(context);
            var cheeps = await cheepRepository.GetCheeps();      
            foreach (CheepDTO cheep in cheeps) 
            {
                Assert.Equal(cheep.Message, "123Testing");
            }
        }
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
        _connection.Dispose();
    }
}