using Microsoft.EntityFrameworkCore;
using Chirp.Core;
using Chirp.Infrastructure;
using System.Globalization;
using FluentValidation;


/// </summary>
/// This class is used as a repostiory of functions/methods that we use to interact with the Database when dealing with Cheeps 
/// This class has methods like getCheeps, Create cheep, and eveything we use later to get or update data that has to do with cheep/cheeps  
/// </summary>
public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext dbContext;

    private IAuthorRepository authorRepository;
    public CheepRepository(ChirpDBContext dbContext, IAuthorRepository authorRepository)
    {
        this.dbContext = dbContext;
        this.authorRepository = authorRepository;
    }

    public async Task<IEnumerable<CheepDTO>> GetCheepsFromAuthors(ICollection<Guid> authorIds, int page = 1, int pageSize = 32)
    {
        IEnumerable<Author> authors = await dbContext.Authors.Include(a => a.Cheeps).Where(a => authorIds.Contains(a.Id)).ToListAsync();

        List<Cheep> Cheeps = new List<Cheep>();

        foreach (Author author in authors)
        {
            Cheeps.AddRange(author.Cheeps);
        }

        var _Cheeps = Cheeps.AsQueryable();

        _Cheeps = _Cheeps.OrderByDescending(t => t.TimeStamp)
            .Skip(CalculateSkippedCheeps(page, pageSize))
            .Take(pageSize)
            .Include(c => c.Author);

        return await CheepsToCheepDTOs(_Cheeps.ToList());
    }

    public async Task<Cheep> GetCheepById(Guid id)
    {
        Cheep cheep = await dbContext.Cheeps.FirstAsync(c => c.Id == id);
        Console.WriteLine(cheep.ToString());
        return cheep;
    }

    public async Task<IEnumerable<CheepDTO>> GetCheeps(int page = 1, int pageSize = 32, string? authorName = null)
    {
        IQueryable<Cheep> Cheeps;

        if (authorName != null)
        {
            Cheeps = (await dbContext.Authors.Include(a => a.Cheeps).FirstAsync(c => c.Name == authorName)).Cheeps.AsQueryable();
        }
        else
        {
            Cheeps = dbContext.Cheeps.Where(c => true);
        }

        Cheeps = Cheeps.OrderByDescending(t => t.TimeStamp)
            .Skip(CalculateSkippedCheeps(page, pageSize))
            .Take(pageSize)
            .Include(c => c.Author);

        return await CheepsToCheepDTOs(Cheeps.ToList());
    }

    public async Task<long> GetCheepsAmount(string? authorName = null)
    {
        long CheepAmount;

        if (authorName != null)
        {
            CheepAmount = dbContext.Cheeps.Where(c => c.Author.Name == authorName).Count();
        }
        else
        {
            CheepAmount = dbContext.Cheeps.Where(c => true).Count();
        }

        return await Task.FromResult(CheepAmount);
    }

    public async Task<int> GetPageAmount(String? authorName = null)
    {
        long cheepAmount;
        if (authorName == null)
        {
            cheepAmount = await GetCheepsAmount();
        }
        else
        {
            cheepAmount = await GetCheepsAmount(authorName);
        }
        return (int)Math.Ceiling((double)cheepAmount / 32);
    }


    public void CreateCheep(createCheepDTO cheepDTO, DateTime? Timestamp = null)
    {
        Author author = dbContext.Authors.First(a => a.Name == cheepDTO.Author.Name);
        if (author == null)
        {
            throw new NullReferenceException("No author was found with name : " + cheepDTO.Author.Name + " email: " + cheepDTO.Author.Email);
        }

        Cheep cheep = new Cheep()
        {
            Author = author,
            Message = cheepDTO.Message,
            TimeStamp = Timestamp == null ? DateTime.Now : (DateTime)Timestamp,
            Reactions = new List<Reaction>(),
        };

        if (author.Cheeps == null)
            author.Cheeps = new List<Cheep>();

        author.Cheeps.Add(cheep);
        dbContext.Cheeps.Add(cheep);
        dbContext.SaveChanges();
    }

    private int CalculateSkippedCheeps(int page, int pageSize)
    {
        if (page < 1)
        {
            page = 1;
        }
        return (page - 1) * pageSize;
    }

    private async Task<IEnumerable<CheepDTO>> CheepsToCheepDTOs(List<Cheep> cheeps)
    {
        var cheepDTOs = new List<CheepDTO>();
        foreach (var cheep in cheeps)
        {
            DateTime cheepDateTimeUtc = cheep.TimeStamp.ToUniversalTime();

            string formattedDateTime = cheepDateTimeUtc.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            ICollection<AuthorDTO>? following = new List<AuthorDTO>();


            ICollection<ReactionDTO> rDTOs = new List<ReactionDTO>();
            foreach (int n in Enum.GetValues(typeof(Reactiontype))) {
                rDTOs.Add(await GetReactions(cheep.Id, n));
            }

            cheepDTOs.Add(new CheepDTO(cheep.Message, cheep.Author.Name, formattedDateTime, rDTOs, following, cheep.Id));
        }

        return cheepDTOs;
    }

    public async Task<ReactionDTO> GetReactions(Guid cheepId, int type)
    {
        return ReactionsToReactionDTO((await dbContext.Cheeps.Include(c => c.Reactions).FirstOrDefaultAsync(c => c.Id == cheepId))!.Reactions, type);
    }

    public async Task ReactToCheep(string? author, string type, Guid cheepId)
    {

        Cheep? cheep = await dbContext.Cheeps.Include(c => c.Reactions).FirstAsync(c => c.Id == cheepId);
        Author? _Author = await dbContext.Authors.Include(a => a.Reactions).FirstOrDefaultAsync(a => a.Name == author);

        if (_Author == null) {
            authorRepository.CreateAuthor(new CreateAuthorDTO(author!, author +"@pmail.com"));
            _Author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == author);
        }

        Console.WriteLine("(String) Author is" + author);
        Console.WriteLine("Author: " + _Author.Name);
        Console.WriteLine("Reactions: " + _Author.Reactions + " Type: " + _Author.Reactions.GetType());
        Reaction reaction = new Reaction()
        {
            ReactionType = type,
            Cheep = cheep,
            Author = _Author
        };
        // Checks if the author has already reacted to the cheep
        if (_Author.Reactions.Where(r => r.Cheep.Id == cheepId).Count() > 0)
        {
            Reaction oldReaction = _Author.Reactions.Where(r => r.Cheep.Id == cheepId).First();
            _Author.Reactions.Remove(oldReaction);
            cheep.Reactions.Remove(oldReaction);
            Console.WriteLine("Removing old reaction of type " + oldReaction.ReactionType + " and adding new reaction of type " + type);
            if (oldReaction.ReactionType == type)
            {
                await dbContext.SaveChangesAsync();
                return;
            }
        }
        cheep.Reactions.Add(reaction);
        _Author.Reactions.Add(reaction);

        await dbContext.SaveChangesAsync();
    }

    public ReactionDTO ReactionsToReactionDTO(ICollection<Reaction> reactions, int type)
    {
        int count = reactions.Where(r => r.ReactionType == ((Reactiontype)type).ToString()).Count();

        return new ReactionDTO((Reactiontype)type, count);
    }


}
