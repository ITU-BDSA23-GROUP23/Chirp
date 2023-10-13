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

    public async Task<AuthorDTO> FindAuthorByEmail(string Email)
    {
        var author = await dbContext.Authors.FindAsync(Email);
        return new AuthorDTO(author.Name, author.Email);
    }

    public async Task<AuthorDTO> FindAuthorByName(string Name)
    {
        var author = await dbContext.Authors.FindAsync(Name);
        return new AuthorDTO(author.Name, author.Email);
    }
}
