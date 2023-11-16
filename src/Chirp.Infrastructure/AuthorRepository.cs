using Azure;
using Chirp.Core;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpDBContext dbContext;


    public AuthorRepository(ChirpDBContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void CreateAuthor(AuthorDTO author)
    {
        dbContext.Authors.Add(new Author
        {
            Name = author.Name,
            Email = author.Email,
            Cheeps = new List<Cheep>()
        });
        dbContext.SaveChanges();
    }


    public async Task<long> GetCheepAmount(string authorName)
    {
        long? CheepAmount;

        Author? Author = await dbContext.Authors.FirstAsync(a => a.Name == authorName);
        if (Author != null)
        {
            CheepAmount = Author.Cheeps.ToList().Count;
            if (CheepAmount == null)
            {
                return 0;
            }
            else
            {
                return (long)CheepAmount;
            }
        }
        else
        {
            throw new NullReferenceException($"Author {authorName} does not exist.");
        }
    }

    public async Task<AuthorDTO?> FindAuthorByEmail(string Email)
    {
        var author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Email == Email);
        if (author != null)
        {
            return new AuthorDTO(author.Name, author.Email);
        }
        return null;
    }

    public async Task<AuthorDTO?> FindAuthorByName(string Name)
    {
        try
        {
            var author = await dbContext.Authors.FirstAsync(a => a.Name == Name);
            return new AuthorDTO(author.Name, author.Email);
        }
        catch (Exception E)
        {
            return null;
        }

        // if (author != null)
        // {
        //     return new AuthorDTO(author.Name, author.Email);
        // }
        // return null;
    }
}