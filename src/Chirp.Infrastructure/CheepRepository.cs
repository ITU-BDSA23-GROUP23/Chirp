using Microsoft.EntityFrameworkCore;
using Chirp.Core;
using Chirp.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Results;


public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext dbContext;

    public CheepRepository(ChirpDBContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<CheepDTO>> GetCheeps(int page = 1, int pageSize = 32, string? authorName = null)
    {
        IQueryable<Cheep> Cheeps;

        if (authorName != null)
        {
            Cheeps = dbContext.Cheeps.Where(c => c.Author.Name == authorName);
        }
        else
        {
            Cheeps = dbContext.Cheeps.Where(c => true);
        }

        Cheeps = Cheeps.OrderByDescending(t => t.TimeStamp)
            .Skip(CalculateSkippedCheeps(page, pageSize))
            .Take(pageSize)
            .Include(c => c.Author);

        return await CheepsToCheepDTOs(Cheeps.ToListAsync());
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

    public void CreateCheep(AuthorDTO Author, string Message)
    {
        Author author = dbContext.Authors.First(a => a.Name == Author.Name);
        if (author == null)
        {
            throw new NullReferenceException("No author was found with name : " + Author.Name + " email: " + Author.Email);
        }

        // Validation
        createCheepDTO cheepDTO = new createCheepDTO(Author, Message);
        createCheepDTOValidator validator = new createCheepDTOValidator();
        ValidationResult results = validator.Validate(cheepDTO);
        if (!results.IsValid)
        {
            foreach (var failure in results.Errors)
            {
                Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
            }
        }

        Cheep cheep = new Cheep()
        {
            Author = author,
            Message = Message,
            TimeStamp = DateTime.Now
        };
        author.Cheeps.Append(cheep);
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

    private async Task<IEnumerable<CheepDTO>> CheepsToCheepDTOs(Task<List<Cheep>> cheeps)
    {
        var cheepDTOs = new List<CheepDTO>();
        foreach (var cheep in await cheeps)
        {
            DateTime cheepDateTimeUtc = cheep.TimeStamp.ToUniversalTime();

            string formattedDateTime = cheepDateTimeUtc.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            cheepDTOs.Add(new CheepDTO(cheep.Message, cheep.Author.Name, formattedDateTime));
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
