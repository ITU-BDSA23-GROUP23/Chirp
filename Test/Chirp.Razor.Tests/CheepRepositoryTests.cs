namespace Chirp.Razor.Tests;
using Xunit;
using Microsoft.Data.Sqlite;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Chirp.Core;
using System.Data.Common;
using Chirp.Web.data;

public class CheepRepositoryTests : IDisposable
{
    ChirpDBContext context;
    SqliteConnection _connection;
    public CheepRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var _contextOptions = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection).Options;
        context = new ChirpDBContext(_contextOptions);
        context.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetCheepsTest()
    {
        IAuthorRepository authorRepository = new AuthorRepository(context);
        ICheepRepository cheepRepository = new CheepRepository(context, authorRepository);
        CreateAuthorDTO author = new CreateAuthorDTO("Jacqualine Gilcoine", "Jacqualine.Gilcoine@gmail.com");
        authorRepository.CreateAuthor(author);
        var Aurthor = authorRepository.FindAuthorByName("Jacqualine Gilcoine").Result;
        createCheepDTO createcheepDTO = new createCheepDTO(Aurthor, "123Testing");
        cheepRepository.CreateCheep(createcheepDTO, null);
        var cheeps = await cheepRepository.GetCheeps();
        foreach (CheepDTO _cheep in cheeps)
        {
            Assert.Equal(_cheep.Message, "123Testing");
        }
    }

    [Fact]
    public async Task GetCheepsAmountTest()
    {
        DbInitializer.Initialize(context);
        var authorRepository = new AuthorRepository(context);
        ICheepRepository cheepRepository = new CheepRepository(context, authorRepository);
        var cheeps = await cheepRepository.GetCheepsAmount("Mellie Yost");
        var authorCheeps = await authorRepository.GetCheepAmount("Mellie Yost");

        Assert.Equal(cheeps, 7);
        Assert.Equal(authorCheeps, 7);
    }

    [Fact]
    public async Task AddReactionTest()
    {
        //Initializing the author
        //Arrange
        IAuthorRepository authorRepository = new AuthorRepository(context);
        CheepRepository _cheepRepository = new CheepRepository(context, authorRepository);
        ICheepRepository cheepRepository = _cheepRepository;
        CreateAuthorDTO author = new CreateAuthorDTO("Jacqualine Gilcoine", "Jacqualine.Gilcoine@gmail.com");
        authorRepository.CreateAuthor(author);
        //Making a cheep
        var Author = authorRepository.FindAuthorByName("Jacqualine Gilcoine").Result;
        createCheepDTO createcheepDTO = new createCheepDTO(Author, "123Testing");
        cheepRepository.CreateCheep(createcheepDTO, null);
        var cheeps = await cheepRepository.GetCheeps();
        // Adding a reaction to the cheep

        // Act
        foreach (var cheep in cheeps)
        {
            Guid id = cheep.Id;
            await cheepRepository.ReactToCheep("Jacqualine Gilcoine", "Like", id);
            var _cheep = _cheepRepository.GetCheepById(id).Result;
            var reaction = _cheep.Reactions.Where(r => r.ReactionType == "Like");

            // Assert
            Assert.Equal(1, reaction.Count());
        }   
    }

    [Fact]
    public async Task RemoveReactionTest()
    {
        // Arrange
        //Initializing the author
        IAuthorRepository authorRepository = new AuthorRepository(context);
        CheepRepository _cheepRepository = new CheepRepository(context, authorRepository);
        ICheepRepository cheepRepository = _cheepRepository;
        CreateAuthorDTO author = new CreateAuthorDTO("Jacqualine Gilcoine", "Jacqualine.Gilcoine@gmail.com");
        authorRepository.CreateAuthor(author);
        //Making a cheep
        var Author = authorRepository.FindAuthorByName("Jacqualine Gilcoine").Result;
        createCheepDTO createcheepDTO = new createCheepDTO(Author, "123Testing");
        cheepRepository.CreateCheep(createcheepDTO, null);
        var cheeps = await cheepRepository.GetCheeps();
        // Adding a reaction to the cheep

        // Act
        foreach (var cheep in cheeps)
        {
            Guid id = cheep.Id;
            await cheepRepository.ReactToCheep("Jacqualine Gilcoine", "Like", id);
            var _cheep = _cheepRepository.GetCheepById(id).Result;
            var reaction = _cheep.Reactions.Where(r => r.ReactionType == "Like");
            Assert.Equal(1, reaction.Count());
        }   

        //Removing the reaction (same method as adding, but since there is already a reaction, we remove it)
        foreach(var cheep in cheeps)
        {
            Guid id = cheep.Id;
            await cheepRepository.ReactToCheep("Jacqualine Gilcoine", "Like", id);
            var _cheep = _cheepRepository.GetCheepById(id).Result;
            var reaction = _cheep.Reactions.Where(r => r.ReactionType == "Like");

            // Assert
            Assert.Equal(0, reaction.Count());
        }
    }

    [Fact]
    public async Task MoveCheepTest()
    {
        // Arrange
        //Initializing the author
        IAuthorRepository authorRepository = new AuthorRepository(context);
        CheepRepository _cheepRepository = new CheepRepository(context, authorRepository);
        ICheepRepository cheepRepository = _cheepRepository;
        CreateAuthorDTO author = new CreateAuthorDTO("Jacqualine Gilcoine", "Jacqualine.Gilcoine@gmail.com");
        authorRepository.CreateAuthor(author);
        //Making a cheep
        var Author = authorRepository.FindAuthorByName("Jacqualine Gilcoine").Result;
        createCheepDTO createcheepDTO = new createCheepDTO(Author, "123Testing");
        cheepRepository.CreateCheep(createcheepDTO, null);
        var cheeps = await cheepRepository.GetCheeps();

        //Act
        // Adding a reaction to the cheep
        foreach (var cheep in cheeps)
        {
            Guid id = cheep.Id;
            await cheepRepository.ReactToCheep("Jacqualine Gilcoine", "Like", id);
            var _cheep = _cheepRepository.GetCheepById(id).Result;
            var reaction = _cheep.Reactions.Where(r => r.ReactionType == "Like");
        }   
        // Moving the cheep
        foreach (var cheep in cheeps)
        {
            Guid id = cheep.Id;
            await cheepRepository.ReactToCheep("Jacqualine Gilcoine", "Dislike", id);
            var _cheep = _cheepRepository.GetCheepById(id).Result;
            var LikeReaction = _cheep.Reactions.Where(r => r.ReactionType == "Like");
            var DislikeReaction = _cheep.Reactions.Where(r => r.ReactionType == "Dislike");

            //Assert 
            Assert.Equal(0, LikeReaction.Count());
            Assert.Equal(1, DislikeReaction.Count());
        }
    }

    public void Dispose()
    {
        context.Dispose();
        _connection.Dispose();
    }
}
