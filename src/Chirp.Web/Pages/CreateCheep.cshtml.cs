using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace Chirp.Web.Pages;



public class CreateCheepModel : PageModel
{


    private readonly ICheepService _service;

    private readonly IAuthorRepository _Author_repository;

    private readonly ICheepRepository _Cheep_repository;
    private readonly ILogger<PublicModel> _logger;

    public CreateCheepModel(ICheepService service, ILogger<PublicModel> logger, IAuthorRepository Author_repository, ICheepRepository Cheep_repository)
    {
        _service = service;
        _logger = logger;
        _Author_repository = Author_repository;
        _Cheep_repository = Cheep_repository;

    }


    public async void createCheep(string UserName, string message)
    {
        var _Author = _Author_repository.FindAuthorByName(UserName);
        _Author.Wait();
        var Author = _Author.Result;
        Console.WriteLine("Author:  " + Author);
        Console.WriteLine("Message:  " + message);
        if (Author != null)
        {
            _Cheep_repository.CreateCheep(Author, message);
        }
        else
        {
            // string email = UserName + "email.com"; // We can't get the correct
            Author = new AuthorDTO(UserName, Email: null);
            _Author_repository.CreateAuthor(Author);
            _Cheep_repository.CreateCheep(Author, message);
        }
    }
    public ActionResult OnGet()
    {

        return Page();


    }
}