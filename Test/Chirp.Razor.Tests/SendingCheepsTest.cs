namespace Chirp.Razor.Tests;
using Xunit;
using Microsoft.Data.Sqlite;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Chirp.Core;
using System.Data.Common;
using Chirp.Web.data;
using System.Threading.Tasks;

public class SendingCheepsTest
{
    ChirpDBContext context;
    SqliteConnection _connection;
    public SendingCheepsTest()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var _contextOptions = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection).Options;
        context = new ChirpDBContext(_contextOptions);
        context.Database.EnsureCreated();
    }

    [Fact]
    public async Task MessageIsNotEmptyTest()
    {
        //Arrange
        string Message = "o";
        ICheepRepository cheepRepository = new CheepRepository(context);
        IAuthorRepository authorRepository = new AuthorRepository(context);
        AuthorDTO author = new AuthorDTO("Thorstein", "tpep123@gmail.com");

        authorRepository.CreateAuthor(author);

        //Act
        cheepRepository.CreateCheep(author, Message);

        //Assert
        var cheeps = await cheepRepository.GetCheeps();
        foreach (var cheepDTO in cheeps)
        {
            if (cheepDTO.Message.Length == 0)
            {
                throw new Exception("Message has length 0");
            }
            else
            {
                Assert.True(cheepDTO.Message.Length <= 1);
            }
        }
    }

    [Fact]
    public async Task EmptyMessageIsNotInsertedTest()
    {
        //Arrange
        string Message = "";
        ICheepRepository cheepRepository = new CheepRepository(context);
        IAuthorRepository authorRepository = new AuthorRepository(context);
        AuthorDTO author = new AuthorDTO("Thorstein", "tpep123@gmail.com");

        authorRepository.CreateAuthor(author);

        //Act
        cheepRepository.CreateCheep(author, Message);
        var cheeps = await cheepRepository.GetCheeps(authorName: author.Name);

        //Assert
        Assert.True(cheeps.Count() == 0);
    }

    [Fact]
    public async Task tooLongMessageIsNotInsertedTest()
    {
        //Arrange
        string Message = "mommopmsemfmpmsopemfpsempfmspklfslfldsnfkldsnfldsnlfnsdlfndslkfndslkfndslfndslfndlskfnkldsnfldsnfldsnfldsnfkldsnflkdsnflkdsknlkadfnvlkfdnvlfnvlksfdnklnfslknsflff";
        ICheepRepository cheepRepository = new CheepRepository(context);
        IAuthorRepository authorRepository = new AuthorRepository(context);
        AuthorDTO author = new AuthorDTO("Thorstein", "tpep123@gmail.com");

        authorRepository.CreateAuthor(author);

        //Act
        cheepRepository.CreateCheep(author, Message);
        var cheeps = await cheepRepository.GetCheeps(authorName: author.Name);

        //Assert
        Assert.True(cheeps.Count() == 0);
    }

    [Fact]
    public async Task CheepMaxLengthTest()
    {
        //Arrange
        string Message = "mommopmsemfmpmsopemfpsempfmspklfslfldsnfkldsnfldsnlfnsdlfndslkfndslkfndslfndslfndlskfnkldsnfldsnfldsnfldsnfkldsnflkdsnflkdsknlkadfnvlkfdnvlfnvlksfdnklnfslknsflf";
        ICheepRepository cheepRepository = new CheepRepository(context);
        IAuthorRepository authorRepository = new AuthorRepository(context);
        AuthorDTO author = new AuthorDTO("Thorstein", "tpep123@gmail.com");

        authorRepository.CreateAuthor(author);

        //Act
        cheepRepository.CreateCheep(author, Message);
        var cheeps = await cheepRepository.GetCheeps(authorName: author.Name);

        //Assert
        Assert.True(cheeps.Count() == 1);
    }

    [Fact]
    public async Task UsingDKletters()
    {
        //Arrange
        string Message = "abcæøå";
        ICheepRepository cheepRepository = new CheepRepository(context);
        IAuthorRepository authorRepository = new AuthorRepository(context);
        AuthorDTO author = new AuthorDTO("Thorstein", "tpep123@gmail.com");

        authorRepository.CreateAuthor(author);

        //Act
        cheepRepository.CreateCheep(author, Message);
        var cheeps = await cheepRepository.GetCheeps();

        //Assert
        foreach (var cheepDTO in cheeps)
        {
            Assert.Contains("æøå", cheepDTO.Message);
        }
    }
}