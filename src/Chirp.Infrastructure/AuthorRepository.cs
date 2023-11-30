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

        Author? Author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == authorName);
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
            return 0;

            // Sometimes the author hasn't been created yet. In that case, we should return 0.
            //throw new NullReferenceException($"Author {authorName} does not exist.");
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

    public AuthorDTO? AuthorToAuthorDTO(Author? author)
    {
        if (author != null)
        {

            ICollection<Guid> Followers = new List<Guid>();
            ICollection<Guid> Following = new List<Guid>();

            if (author.Followers != null)
            {
                foreach (var follower in author.Followers)
                {
                    Followers.Add(follower.Id);
                }
            }

            if (author.Following != null)
            {
                foreach (var following in author.Following)
                {
                    Following.Add(following.Id);
                }
            }

            Console.WriteLine($"Created AuthorDTO for {author.Name} with {Followers.Count} followers and following {Following.Count}");
            return new AuthorDTO(author.Name, author.Email, Followers, Following);
        }
        return null;
    }

    public async Task<AuthorDTO?> FindAuthorByName(string Name)
    {
        var author = await dbContext.Authors.Include(f => f.Followers).Include(f => f.Following).FirstOrDefaultAsync(a => a.Name == Name);
        return AuthorToAuthorDTO(author);


        // if (author != null)
        // {
        //     return new AuthorDTO(author.Name, author.Email);
        // }
        // return null;
    }

    //Copilot with this!
    public async Task UnfollowAuthor(AuthorDTO other, AuthorDTO self)
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

    public async Task FollowAuthor(AuthorDTO other, AuthorDTO self)
    {
        Console.WriteLine("Other name is " + other.Name);

        var selfAuthor = await dbContext.Authors.FirstAsync(a => a.Name == self.Name);
        var otherAuthor = await dbContext.Authors.FirstAsync(a => a.Name == other.Name);
        Console.WriteLine("My name is: " + selfAuthor.Name);
        if (selfAuthor != null && otherAuthor != null)
        {
            Console.WriteLine("I am here!");
            selfAuthor.Following.Add(otherAuthor);
            otherAuthor.Followers.Add(selfAuthor);
            Console.WriteLine($"Follow author called for {otherAuthor.Name}. They now have {otherAuthor.Followers.Count} followers!");
            await dbContext.SaveChangesAsync();
        }
        else
        {
            throw new NullReferenceException($"Author {self.Name} or {other.Name} does not exist.");
        }
    }

    public async Task<IEnumerable<AuthorDTO>> GetFollowers(string authorName)
    {
        var author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == authorName);
        if (author != null)
        {
            ICollection<AuthorDTO> Followers = new List<AuthorDTO>();
            foreach (var follower in author.Followers)
            {
                Followers.Add(AuthorToAuthorDTO(follower));
            }
            return Followers;
        }
        return null;
    }
    public async Task<IEnumerable<AuthorDTO>> GetFollowing(string authorName)
    {
        var author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == authorName);
        if (author != null)
        {
            ICollection<AuthorDTO> Following = new List<AuthorDTO>();
            foreach (var following in author.Following)
            {
                Following.Add(AuthorToAuthorDTO(following));
            }
            return Following;
        }
        return null;
    }

    public async Task DeleteAuthor(string authorName)
    {
        var author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == authorName);
        if (author != null)
        {
            // Delete the Cheeps
            dbContext.Cheeps.RemoveRange(dbContext.Cheeps.Where(c => c.Author.Name == authorName));
            // Delete the Author
            dbContext.Authors.Remove(author);
            await dbContext.SaveChangesAsync();
        }
        else
        {
            throw new NullReferenceException($"Author {authorName} does not exist.");
        }
    }
}