using Azure;
using Chirp.Core;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Chirp.Infrastructure;
using SQLitePCL;

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpDBContext dbContext;


    public AuthorRepository(ChirpDBContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void CreateAuthor(CreateAuthorDTO author)
    {
        dbContext.Authors.Add(new Author
        {
            Name = author.Name,
            Email = author.Email,
            Cheeps = new List<Cheep>(),
            Following = new List<Author>(),
            Followers = new List<Author>()
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
            return AuthorToAuthorDTO(author);
        }
        return null;
    }

    public AuthorDTO? AuthorToAuthorDTO(Author author)
    {
        if (author != null)
        {

            ICollection<Guid> Followers = new List<Guid>();
            ICollection<Guid> Following = new List<Guid>();

            foreach (var follower in author.Followers)
            {
                Followers.Add(follower.Id);
            }

            foreach (var following in author.Following)
            {
                Following.Add(following.Id);
            }

            return new AuthorDTO(author.Name, author.Email, Followers, Following);
        }
        return null;
    }

    public async Task<AuthorDTO?> FindAuthorByName(string Name)
    {
        try
        {
            var author = await dbContext.Authors.FirstAsync(a => a.Name == Name);
            return AuthorToAuthorDTO(author);
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

    //Copilot with this!
    public async Task UnfollowAuthor(AuthorDTO self, AuthorDTO other)
    {
        var selfAuthor = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == self.Name);
        var otherAuthor = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == other.Name);

        if (selfAuthor != null && otherAuthor != null)
        {
            selfAuthor.Following.Remove(otherAuthor);
            otherAuthor.Followers.Remove(selfAuthor);
            await dbContext.SaveChangesAsync();
        }
        else
        {
            throw new NullReferenceException($"Author {self.Name} or {other.Name} does not exist.");
        }
    }

    public async Task FollowAuthor(AuthorDTO self, AuthorDTO other)
    {
        var selfAuthor = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == self.Name);
        var otherAuthor = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == other.Name);

        if (selfAuthor != null && otherAuthor != null)
        {
            selfAuthor.Following.Add(otherAuthor);
            otherAuthor.Followers.Add(selfAuthor);
            await dbContext.SaveChangesAsync();
        }
        else
        {
            throw new NullReferenceException($"Author {self.Name} or {other.Name} does not exist.");
        }
    }
}