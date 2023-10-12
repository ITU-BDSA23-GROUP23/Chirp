using Azure;
using Chirp.Razor;
using Chirp.Razor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories;
using NuGet.Protocol.Plugins;

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
            Name = author.GetName(),
            Email = author.GetEmail()
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
