namespace Chirp.Razor.Tests;
using Xunit;
using System.Net;
using Chirp.Web.data;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class CheepRepositoryTests
{

    [Fact]
    public void CreateCheepTest()
    {
        
    }

    [Fact]
    public void GetCheepsTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ChirpDBContext>().UseInMemoryDatabase("Data source = /tmp/chirp.db");
        using (var db = new ChirpDBContext(optionsBuilder.Options))
        {
              
        }
    }

    [Fact]
    public void CalculateSkippedCheepsTest()
    {

    }

    [Fact]
    public void CheepsToCheepDTOsTest()
    {

    }


}
