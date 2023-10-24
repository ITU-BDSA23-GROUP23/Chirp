namespace Chirp.Razor.Tests;
using Xunit;
using System.Net;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class AuthorRepositoryTests
{
    [Fact(Skip = "kk")]
    public void CreateAuthorTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ChirpDBContext>().UseInMemoryDatabase("CheepMemoryDb");
        using var db = new ChirpDBContext(optionsBuilder.Options);
        //Arrange
        AuthorDTO authorDTO = new("Thorbjørnen", "tpep@bjørn.dk");
        AuthorRepository authorRepository = new(db);

        //Act
        authorRepository.CreateAuthor(authorDTO);

        //Assert
        var addedAuthor = db.Authors.SingleOrDefault(a => a.Name == authorDTO.Name && a.Email == authorDTO.Email);
        Assert.NotNull(addedAuthor);
    }

    [Fact(Skip = "kk")]
    public void FindAuthorByEmail()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ChirpDBContext>().UseInMemoryDatabase("CheepMemoryDb");
        using var db = new ChirpDBContext(optionsBuilder.Options);
        //Arrange
        AuthorDTO authorDTO = new("Thorbjørnen1", "tpep1@bjørn.dk");
        AuthorRepository authorRepository = new(db);

        //Act
        _ = authorRepository.FindAuthorByEmail(authorDTO.Email);

        //Assert
        var addedAuthor = db.Authors.SingleOrDefault(a => a.Email == authorDTO.Email);
        Assert.NotNull(addedAuthor);
        Assert.True(addedAuthor.Email == "tpep1@bjørn.dk");
    }

    [Fact(Skip = "kk")]
    public void FindAuthorByName()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ChirpDBContext>().UseInMemoryDatabase("CheepMemoryDb");
        using var db = new ChirpDBContext(optionsBuilder.Options);
        //Arrange
        AuthorDTO authorDTO = new("Thorbjørnen2", "tpep2@bjørn.dk");
        AuthorRepository authorRepository = new(db);

        //Act
        _ = authorRepository.FindAuthorByName(authorDTO.Name);

        //Assert
        var addedAuthor = db.Authors.SingleOrDefault(a => a.Name == authorDTO.Name);
        db.Authors.Add(addedAuthor);
        Assert.NotNull(addedAuthor);
        Assert.True(addedAuthor.Name == "Thorbjørnen2");
    }

}
