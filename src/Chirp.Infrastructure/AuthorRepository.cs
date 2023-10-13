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
            Email = author.Email
        });
    }

    public AuthorDTO FindAuthorByEmail(string Email)
    {
        var author = dbContext.Authors.Find(Email);
        return new AuthorDTO(author.Name, author.Email);
    }

    public AuthorDTO FindAuthorByName(string Name)
    {
        var author = dbContext.Authors.Find(Name);
        return new AuthorDTO(author.Name, author.Email);
    }
}
