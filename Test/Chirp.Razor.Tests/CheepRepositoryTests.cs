namespace Chirp.Razor.Tests;
using Xunit;
using System.Net;
using Chirp.Web.data;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Chirp.Core;

public class CheepRepositoryTests
{

    [Fact]
    public void CreateCheepTest()
    {
        
    }

    [Fact]
    public async Task GetCheepsTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ChirpDBContext>().UseInMemoryDatabase("CheepMemoryDb");
        using (var db = new ChirpDBContext(optionsBuilder.Options))
        {
    
            var a10 = new Author() { Name = "Jacqualine Gilcoine", Email = "Jacqualine.Gilcoine@gmail.com", Cheeps = new List<Cheep>() };
        
            var c1 = new Cheep() { Author = a10, Message = "123Testing", TimeStamp = DateTime.Parse("2023-08-01 13:14:37") };
        
            db.Authors.Add(a10);
            db.Cheeps.Add(c1);
            db.SaveChanges();
            
            var cheepRepository = new CheepRepository(db);

            var cheeps = await cheepRepository.GetCheeps();           
            
            foreach (CheepDTO cheep in cheeps) 
            {
                Assert.Equal(cheep.Message, "123Testing");
            }
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
