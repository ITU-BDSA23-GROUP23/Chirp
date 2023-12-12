using Chirp.Core;
using Microsoft.EntityFrameworkCore;
using Chirp.Infrastructure;


/// <summary>
/// This class is used as a repostiory of functions/methods that we use to interact with the Database when dealing with Authors 
/// This class has methods like findAuthor, Create author, and eveything we use later to get or update data that has to do with an Author  
/// </summary>

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
            Followers = new List<Author>(),
            Reactions = new List<Reaction>()
        });
        dbContext.SaveChanges();
    }

    public async Task<string> GetAuthorName(Guid id)
    {
        return (await dbContext.Authors.FirstAsync(a => a.Id == id)).Name;
    }

    public async Task<long> GetCheepAmount(string authorName)
    {
        long? CheepAmount;

        Author? Author = await dbContext.Authors.Include(a => a.Cheeps).FirstOrDefaultAsync(a => a.Name == authorName);
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
            // OLD //throw new NullReferenceException($"Author {authorName} does not exist.");
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

    // public async Task AddReaction(string type, Guid CheepId, string authorName)
    // {
    //     var cheep = await dbContext.Cheeps.Include(c => c.Reactions).FirstAsync(c => c.Id == CheepId);
    //     var reaction = new Reaction()
    //     {
    //         Author = await dbContext.Authors.FirstAsync(a => a.Name == authorName),
    //         ReactionType = type,
    //         Cheep = cheep
    //     };
    //     cheep.Reactions.Add(reaction);
    // }

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
            return new AuthorDTO(author.Name, author.Email!, Followers, Following, author.Id);
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
        var selfAuthor = await dbContext.Authors.Include(f => f.Following).FirstAsync(a => a.Name == self.Name);
        var otherAuthor = await dbContext.Authors.Include(f => f.Followers).FirstAsync(a => a.Name == other.Name);
        Console.WriteLine($"selfauthor followinglist? {otherAuthor.Following.Count}");

        if (selfAuthor != null && otherAuthor != null)
        {
            selfAuthor.Following.Remove(selfAuthor.Following.FirstOrDefault(f => f.Name == other.Name)!);
            otherAuthor.Followers.Remove(otherAuthor.Followers.FirstOrDefault(f => f.Name == self.Name)!);
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

        var author = await dbContext.Authors.Include(a => a.Followers).FirstOrDefaultAsync(a => a.Name == authorName);
        ICollection<AuthorDTO> Followers = new List<AuthorDTO>();
        if (author != null)
        {
            foreach (var follower in author.Followers)
            {
                Console.WriteLine("Adding follower");
                Followers.Add(AuthorToAuthorDTO(follower)!);
            }
        }
        return Followers;
    }

    public async Task<IEnumerable<AuthorDTO>> GetFollowing(string authorName)
    {
        var author = await dbContext.Authors.Include(a => a.Following).FirstOrDefaultAsync(a => a.Name == authorName);
        ICollection<AuthorDTO> Following = new List<AuthorDTO>();
        if (author != null)
        {
            foreach (var following in author.Following)
            {
                Console.WriteLine("Adding following");
                Following.Add(AuthorToAuthorDTO(following)!);
            }
            Console.WriteLine("Returning following");
        }
        Console.WriteLine("Returning null");
        return Following;
    }

    public async Task ForgetMe(string authorName)
    {
        // For testing: var Author = await dbContext.Authors.Include(r => r.Reactions).FirstOrDefaultAsync(a => a.Name == authorName);
        // For testing: Console.WriteLine($"Author {authorName} has {Author.Reactions.Count} reactions.");
        var _Followers = await GetFollowers(authorName);
        var _Following = await GetFollowing(authorName);
        Console.WriteLine("Followers: (" + _Followers + ")");
        Console.WriteLine("Following: (" + _Following + ")");

        if (_Followers != null)
        {
            Console.WriteLine("Deleting follolwers");
            await RemoveFollowers(_Followers, authorName);
            Console.WriteLine($"Followers deleted.");
        }
        if (_Following != null)
        {
            Console.WriteLine("Deleting following");
            await RemoveFollowing(_Following, authorName);
            Console.WriteLine($"Following deleted.");
        }

        // Deleting reactions connected to the author.
        await DeleteReactionsForCheepsByAuthor(authorName);
        await DeleteReactionsByAuthor(authorName);

        // Deleting cheeps connected to the author, and the author itself.
        await DeleteAuthorCheeps(authorName);
        await DeleteAuthor(authorName);
        Console.WriteLine($"Author {authorName} deleted.");
    }

    public async Task DeleteAuthor(string authorName)
    {
        var author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == authorName);
        if (author != null)
        {
            // Delete the Author
            dbContext.Authors.Remove(author);
            await dbContext.SaveChangesAsync();
        }
        else
        {
            // throw new NullReferenceException($"Author {authorName} does not exist.");
        }
    }

    public async Task DeleteAuthorCheeps(string authorName)
    {
        dbContext.Cheeps.RemoveRange(dbContext.Cheeps.Where(c => c.Author.Name == authorName));
        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveFollowers(IEnumerable<AuthorDTO> result, string DeletingAuthorName)
    {
        var authorNames = result.Select(author => author.Name).ToList();
        var authors = await dbContext.Authors
            .Where(a => authorNames.Contains(a.Name))
            .ToListAsync();

        // Deletes the author that is trying to delete itselft from the app, from each of its followers' following lists.
        foreach (var author in authors)
        {
            author.Following.Remove(author.Following.FirstOrDefault(f => f.Name == DeletingAuthorName)!);
        }

        var DelitingAuthor = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == DeletingAuthorName);

        DelitingAuthor!.Followers.Clear();

        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveFollowing(IEnumerable<AuthorDTO> result, string DeletingAuthorName)
    {
        var authorNames = result.Select(author => author.Name).ToList();
        var authors = await dbContext.Authors
            .Where(a => authorNames.Contains(a.Name))
            .ToListAsync();

        foreach (var author in authors)
        {
            author.Followers.Remove(author.Followers.FirstOrDefault(f => f.Name == DeletingAuthorName)!);
        }

        var DelitingAuthor = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == DeletingAuthorName);

        DelitingAuthor!.Following.Clear();
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteReactionsForCheepsByAuthor(string AuthorName)
    {
        var Cheeps = await dbContext.Cheeps.Where(c => c.Author.Name == AuthorName).ToListAsync();
        // We get the reaction for each cheep, then get the author of the reaction, and then remove the reaction from the author's reactions.
        foreach (var cheep in Cheeps)
        {
            var Reactions = await dbContext.Reactions.Where(r => r.Cheep.Id == cheep.Id).ToListAsync();
            foreach (var reaction in Reactions)
            {
                var Author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Reactions.Contains(reaction));
                Author.Reactions.Remove(reaction);
            }
        }
        // Deletes the reactions from the database.
        foreach (var cheep in Cheeps)
        {
            dbContext.Reactions.RemoveRange(dbContext.Reactions.Where(r => r.Cheep.Id == cheep.Id));
        }
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteReactionsByAuthor(string AuthorName)
    {
        Console.WriteLine($"Deleting reactions by author {AuthorName}");
        var Author = await dbContext.Authors.Include(r => r.Reactions).FirstOrDefaultAsync(a => a.Name == AuthorName);
        if (Author != null)
        {
            Console.WriteLine($"Author {AuthorName} has {Author.Reactions.Count} reactions.");
            var reactionsToRemove = dbContext.Reactions.Include(c => c.Cheep).Include(a => a.Author).Where(r => r.Author.Name == AuthorName);
            Author.Reactions = new List<Reaction>();
            // We remove the specific reactions from their cheeps.
            foreach (var reaction in reactionsToRemove)
            {
                Console.WriteLine($"Removing reaction {reaction.ReactionId} from cheep {reaction.Cheep.Id}");
                reaction.Cheep.Reactions.Remove(reaction);
            }

            dbContext.Reactions.RemoveRange(reactionsToRemove);

            // Author.Reactions.Clear(); For some reason, this doesn't work. Instead we set Reactions to a new empty list.

            await dbContext.SaveChangesAsync();
        }
    }

    public int GetFollowingCount(string authorName)
    {
        var _author = FindAuthorByName(authorName).Result;
        if (_author == null)
        {
            return 0;
        }
        else
        {
            int FollowingCount = _author?.Following?.Count ?? 0;
            return FollowingCount;
        }
    }

    public int GetFollowersCount(string authorName)
    {
        var _author = FindAuthorByName(authorName).Result;
        if (_author == null)
        {
            return 0;
        }
        else
        {
            int FollowersCount = _author?.Followers?.Count ?? 0;
            return FollowersCount;
        }
    }

    public bool IsFollowing(string authorNameOther, string authorNameSelf)
    {
        var _authorOther = FindAuthorByName(authorNameOther).Result;
        var _authorSelf = FindAuthorByName(authorNameSelf).Result;
        if (_authorSelf!.Following?.Contains(_authorOther!.Id) ?? false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

