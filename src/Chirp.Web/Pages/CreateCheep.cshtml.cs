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


    private readonly IAuthorRepository _Author_repository;

    private readonly ICheepRepository _Cheep_repository;

    public CreateCheepModel(IAuthorRepository Author_repository, ICheepRepository Cheep_repository)
    {
        _Author_repository = Author_repository;
        _Cheep_repository = Cheep_repository;

    }

    public ActionResult OnPostCheep(string c, string message) 
    {
        if (c != null)
        { 
            createCheep(c, message);
        }

        return RedirectToPage("/");
    }

    public void createCheep(string UserName, string? message)

    {
        var _Author = _Author_repository.FindAuthorByName(UserName);
        _Author.Wait();
        var Author = _Author.Result;

  
        if (Author != null)
        {

            createCheepDTO cheepDTO = new createCheepDTO(Author, message);
            createCheepDTOValidator validator = new createCheepDTOValidator();
            FluentValidation.Results.ValidationResult results = validator.Validate(cheepDTO);
            if (!results.IsValid)
            {
                foreach (var failure in results.Errors)
                {
                    Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                }
            }
            else {
            _Cheep_repository.CreateCheep(cheepDTO, null);
            }
        }
        else
        {
            string email = UserName + "email.com";

            _Author_repository.CreateAuthor(new CreateAuthorDTO(UserName, email));
            Author = _Author_repository.FindAuthorByName(UserName).Result;
            createCheepDTO cheepDTO = new createCheepDTO(Author, message);
            createCheepDTOValidator validator = new createCheepDTOValidator();
            FluentValidation.Results.ValidationResult results = validator.Validate(cheepDTO);
            if (!results.IsValid)
            {
                foreach (var failure in results.Errors)
                {
                    Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                }
            } else {
                _Cheep_repository.CreateCheep(cheepDTO, null);
            }
            
        }
    }
    public ActionResult OnGetPartial()
    {
       return Page();
    }
}
