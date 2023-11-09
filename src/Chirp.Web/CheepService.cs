using Chirp.Core;
using Chirp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICheepService : IDisposable
{
    Task<IEnumerable<CheepDTO>> GetCheeps(int page);
    Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string authorName, int page);

    void createCheep(string UserName, string message);

    public Task<int> GetPageAmount(String? authorName = null);
}

public class CheepService : ICheepService
{
    int PageSize = 32; // This is only used in CheepService, not by the Repositeries that it uses, at this point. In the future it should be used in the repositories.
    private readonly ICheepRepository cheepRepository;
    private readonly IAuthorRepository authorRepository;

    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        this.cheepRepository = cheepRepository;
        this.authorRepository = authorRepository;
    }

    public async Task<IEnumerable<CheepDTO>> GetCheeps(int page)
    {
        return await cheepRepository.GetCheeps(page);

    }

    public async Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string authorName, int page)
    {
        return await cheepRepository.GetCheeps(page, authorName: authorName);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

    public async Task<int> GetPageAmount(String? authorName = null)
    {
        long cheepAmount;
        if (authorName == null)
        {
            cheepAmount = await cheepRepository.GetCheepsAmount();
        }
        else
        {
            cheepAmount = await authorRepository.GetCheepAmount(authorName);
        }
        return (int)Math.Ceiling((double)cheepAmount / PageSize);
    }

    public async void createCheep(string UserName, string message) {
        
        var Author = await this.authorRepository.FindAuthorByName(UserName); 
        if(Author!= null) {
            this.cheepRepository.CreateCheep(Author, message);
        } else {
            string email = UserName + "email.com";
            Author = new AuthorDTO(UserName, email);
            this.authorRepository.CreateAuthor(Author);
        }
        

    }

    public void Dispose()
    {
        // Implement Dispose method here if necessary.
    }
}
