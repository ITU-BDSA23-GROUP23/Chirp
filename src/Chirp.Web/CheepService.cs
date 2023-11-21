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
    int GetFollowingCount(string author);
    int GetFollowersCount(string author);
}

public class CheepService : ICheepService
{
    int PageSize = 32; // This is only used in CheepService, not by the Repositeries that it uses, at this point. In the future it should be used in the repositories.
    private readonly ICheepRepository cheepRepository;
    private readonly IAuthorRepository authorRepository;

    ChirpDBContext _dbContext;

    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository, ChirpDBContext dbContext)
    {
        this.cheepRepository = cheepRepository;
        this.authorRepository = authorRepository;
        _dbContext = dbContext;
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

    public async void createCheep(string UserName, string message)
    {

        var Author = await this.authorRepository.FindAuthorByName(UserName);
        if (Author != null)
        {
            this.cheepRepository.CreateCheep(Author, message);
        }
        else
        {
            string email = UserName + "email.com";
            Author = new AuthorDTO(UserName, email);
            this.authorRepository.CreateAuthor(Author);
        }

    }

    public void Dispose()
    {
        // Implement Dispose method here if necessary.
    }

    public int GetFollowingCount(string authorName)
    {
        Author author = _dbContext.Authors.First(a => a.Name == authorName);

        if (author != null)
        {
            // Check if author.Following is not null before accessing Count
            int followingCount = author.Following?.Count ?? 0;
            return followingCount;
        }

        return 0; // or handle the case when the author is not found
    }

    public int GetFollowersCount(string authorName)
    {
        Author author = _dbContext.Authors.First(a => a.Name == authorName);

        if (author != null)
        {
            // Check if author.Followers is not null before accessing Count
            int followersCount = author.Followers?.Count ?? 0;
            return followersCount;
        }

        return 0; // or handle the case when the author is not found
    }

}
