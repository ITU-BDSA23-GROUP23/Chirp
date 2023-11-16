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
            Cheeps = new List<Cheep>(),
            Following = new List<Author>(),
            Followers = new List<Author>()
        });
        dbContext.SaveChanges();
    }
    public async Task FollowAuthor(string followerName, string followingName)
    {
        Author? follower = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == followerName);
        Author? following = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == followingName);

        if (follower != null && following != null)
        {
            follower.Following.Add(following);
            following.Followers.Add(follower);
            dbContext.SaveChanges();
        }
        else
        {
            throw new NullReferenceException($"Author {followerName} or {followingName} does not exist.");
        }
    }

    public async Task UnfollowAuthor(string followerName, string followingName)
    {
        Author? follower = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == followerName);
        Author? following = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == followingName);

        if (follower != null && following != null)
        {
            follower.Following.Remove(following);
            following.Followers.Remove(follower);
            dbContext.SaveChanges();
        }
        else
        {
            throw new NullReferenceException($"Author {followerName} or {followingName} does not exist.");
        }
    }


    public async Task<long> GetCheepAmount(string authorName)
    {
        long CheepAmount;

        Author? Author = await dbContext.Authors.FirstAsync(a => a.Name == authorName);
        if (Author != null)
        {
            CheepAmount = Author.Cheeps.ToList().Count;
            return CheepAmount;
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