namespace Chirp.Razor.Tests;
using Xunit;
using Microsoft.Data.Sqlite;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;


public class CheepRepositoryTests
{
    public CheepRepositoryTests()
    {
        var _connection =  new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var _contextOptions = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection).Options;

        using var context = new ChirpDBContext(_contextOptions);
        context.SaveChanges();
    }
}