using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Web.Pages.Shared;

namespace Chirp.Web.Pages;

public class PageInfoModel : PageModel
{
    private readonly IAuthorRepository _Author_repository;
    private readonly ICheepRepository _Cheep_repository;

    public AuthorDTO? Author { get; set; }
    public IEnumerable<CheepDTO>? Cheeps { get; set; }
    public IEnumerable<AuthorDTO>? Followers { get; set; }
    public IEnumerable<AuthorDTO>? Following { get; set; }
    public long CheepAmount { get; set; }

    private readonly ILogger<PublicModel> _logger;

    public PageInfoModel(ILogger<PublicModel> logger, IAuthorRepository Author_repository, ICheepRepository Cheep_repository)
    {
        _logger = logger;
        _Author_repository = Author_repository;
        _Cheep_repository = Cheep_repository;
    }

    // get author name and email
    public async Task OnGetAuthor(string author)
    {
        Console.WriteLine("author is " + author + " in OnGetAuthor");
        var _Author = await _Author_repository.FindAuthorByName(author);
        Author = _Author;
    }

    // list of cheeps
    public async Task<ActionResult> OnGet(string authorName, [FromQuery] int page)
    {
        authorName = User.Identity?.Name!;
        try
        {
            var _Cheeps = _Cheep_repository.GetCheeps(authorName: authorName);
            _Cheeps.Wait();
            Cheeps = _Cheeps.Result;
            Console.WriteLine(authorName + "54321");
            await OnGetFollowers(authorName);
            await OnGetFollowing(authorName);
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
        }
        return Page();

    }



    public CheepModel GenerateCheepModel(CheepDTO cheep)
    {
        return new CheepModel(_Author_repository, _Cheep_repository, cheep);
    }

    public int FollowingCount(string authorName)
    {
        return _Author_repository.GetFollowingCount(authorName);
    }

    public int FollowersCount(string authorName)
    {
        return _Author_repository.GetFollowersCount(authorName);
    }

    public async Task OnGetFollowers(string? authorName)
    {
        var _Followers = await _Author_repository.GetFollowers(authorName!);
        Followers = _Followers;
    }

    public async Task OnGetFollowing(string authorName)
    {
        Console.WriteLine("OnGetFollowing called");
        var _Following = await _Author_repository.GetFollowing(authorName);
        Following = _Following;
    }

    // This method is run, when the 'Forget me' button is pressed.
    public async Task<IActionResult> OnPost([FromQuery] string h)
    {
        if (h == "h")
        {
            Console.WriteLine("OnPostDeleteAuthor called.");

            // I want to make a break for 10 seconds here

            string authorName = User.Identity?.Name!; // Author.Name;

            var _Author = await _Author_repository.FindAuthorByName(authorName);
            if (_Author != null)
            {
                await _Author_repository.ForgetMe(authorName);
            }
            else
            {
                // Console.WriteLine("No author found with name " + authorName);
            }
            return Redirect("~/MicrosoftIdentity/Account/SignOut");
        }
        return Page();
    }
}
