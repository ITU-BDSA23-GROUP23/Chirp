using Chirp.Razor;
using Chirp.Razor.Models;
using Microsoft.Identity.Client;


public interface ICheepService
{
    public List<Cheep> GetCheeps(int? page);
    // public List<DBFacade.CheepViewModel> GetCheepsFromAuthor(string author);
    public List<Cheep> GetCheepsFromAuthor(int test);
}

public class CheepService : ICheepService
{
    private readonly ChirpDBContext context;
    public CheepService(ChirpDBContext context) {
        this.context = context;
        //context = new();
    }
    public List<Cheep> GetCheeps(int? page)
    {
        // context.Add(new Cheep{
        //     Author = new Author{
        //     Name = "TesterGuy",
        //     Email = "TesterGuy@TestMail.test"},
        //     Message = "Virkelig sej ting",
        //     TimeStamp = DateTime.Now,
        // });
        // context.SaveChanges();


        return context.Cheeps.ToList();
    }



    public List<Cheep> GetCheepsFromAuthor(int test) {

       

        return context.Cheeps.Where(r => r.Id == test).ToList();
        
        
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

}
