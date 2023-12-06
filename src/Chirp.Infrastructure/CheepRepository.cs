using Microsoft.EntityFrameworkCore;
using Chirp.Core;
using Chirp.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Results;

/// This class is used as a repostiory of functions/methods that we use to interact with the Database when dealing with Cheeps 
/// This class has methods like getCheeps, Create cheep, and eveything we use later to get or update data that has to do with cheep/cheeps  
public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext dbContext;

    public CheepRepository(ChirpDBContext dbContext)
    {
        this.dbContext = dbContext;
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

        return CheepsToCheepDTOs(_Cheeps.ToList());
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

        return CheepsToCheepDTOs(Cheeps.ToList());
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

        return CheepAmount;
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
            TimeStamp = Timestamp == null ? DateTime.Now : (DateTime)Timestamp
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

    private IEnumerable<CheepDTO> CheepsToCheepDTOs(List<Cheep> cheeps)
    {
        var cheepDTOs = new List<CheepDTO>();
        foreach (var cheep in cheeps)
        {
            DateTime cheepDateTimeUtc = cheep.TimeStamp.ToUniversalTime();

            string formattedDateTime = cheepDateTimeUtc.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            ICollection<AuthorDTO>? following = new List<AuthorDTO>();

            cheepDTOs.Add(new CheepDTO(cheep.Message, cheep.Author.Name, formattedDateTime, (ICollection<ReactionDTO>?)cheep.Reactions, following));
        }

        return cheepDTOs;
    }

    // public void Add(Cheep entity)
    // {
    //     dbContext.Set<Cheep>().Add(entity);
    //     dbContext.SaveChanges();
    // }

    // public void Update(Cheep entity)
    // {
    //     dbContext.Set<Cheep>().Update(entity);
    //     dbContext.SaveChanges();
    // }

    // public void Remove(Cheep entity)
    // {
    //     dbContext.Set<Cheep>().Remove(entity);
    //     dbContext.SaveChanges();
    // }
}
