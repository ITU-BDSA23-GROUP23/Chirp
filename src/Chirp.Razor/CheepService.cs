using Chirp.Razor;
using Chirp.Razor.Models;
using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


public interface ICheepService
{
    public List<Cheep> GetCheeps(int? page);
    // public List<DBFacade.CheepViewModel> GetCheepsFromAuthor(string author);
    public List<Cheep> GetCheepsFromAuthor(string authorName);
}

public class CheepService : ICheepService
{
    private readonly int PageLimit = 16;
    private readonly ChirpDBContext context;
    public CheepService(ChirpDBContext context)
    {
        this.context = context;
        //context = new();
    }
    public List<Cheep> GetCheeps(int? page)
    {
        var skip = 0;
        // context.Add(new Cheep{
        //     Author = new Author{
        //     Name = "TesterGuy",
        //     Email = "TesterGuy@TestMail.test"},
        //     Message = "Virkelig sej ting",
        //     TimeStamp = DateTime.Now,
        // });
        // context.SaveChanges();
        if (page != null)
            skip = (int)((page - 1) * PageLimit);

        var cheeps = context.Cheeps.OrderByDescending(t => t.TimeStamp).Skip(skip).Take(PageLimit).Include(c => c.Author);

        return cheeps.ToList();
    }



    public List<Cheep> GetCheepsFromAuthor(string authorName)
    {

        Console.WriteLine(authorName);
        return context.Cheeps.Where(r => r.Author.Name == authorName).Include(c => c.Author).ToList();


    }


    // public List<DBFacade.CheepViewModel> GetCheepsFromAuthor(string author)
    // {
    //     // filter by the provided author name
    //     return dBFacade.GetCheepsFromAuthor(author);
    // }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
