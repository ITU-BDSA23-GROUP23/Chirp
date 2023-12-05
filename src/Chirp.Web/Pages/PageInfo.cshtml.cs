using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

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
        var _Author = _Author_repository.FindAuthorByName(author);
        _Author.Wait();
        Author = _Author.Result;
    }

    // list of cheeps
    public async Task<ActionResult> OnGet(string authorName, [FromQuery] int page)
    {
        authorName = User.Identity.Name;
        var _Cheeps = _Cheep_repository.GetCheeps(authorName: authorName);
        _Cheeps.Wait();
        Cheeps = _Cheeps.Result;
        Console.WriteLine(authorName + "54321");
        await OnGetFollowers(authorName);
        await OnGetFollowing(authorName);
        return Page();
    }

    public int FollowingCount(string authorName)
    {
        var _author = _Author_repository.FindAuthorByName(authorName).Result;
        Console.WriteLine($"Is author null? = {_author == null}");
        //Console.WriteLine(_author.Following.ToArray()[0]);    
        int _FollowingCount = _author?.Following?.Count ?? 0;
        Console.WriteLine(_FollowingCount + " is the result of _FollowingCount");
        return _FollowingCount;
    }

    public int FollowersCount(string authorName)
    {
        var _author = _Author_repository.FindAuthorByName(authorName);
        _author.Wait();
        if (_author.Result == null)
        {
            Console.WriteLine("Author is null");
            return 0;
        }
        else
        {
            Console.WriteLine("Author is not null1");
            var _FollowersCount = _author.Result.Followers.Count;
            Console.WriteLine("Author is not null2");
            Console.WriteLine(_FollowersCount + " is the result of _FollowersCount");
            return _FollowersCount;
        }
    }

    public async Task OnGetFollowers(string authorName)
    {
        var _Followers = _Author_repository.GetFollowers(authorName);
        _Followers.Wait();
        Followers = _Followers.Result;
    }

    public async Task OnGetFollowing(string authorName)
    {
        Console.WriteLine("OnGetFollowing called");
        var _Following = _Author_repository.GetFollowing(authorName);
        _Following.Wait();
        Following = _Following.Result;
    }

    // This method is run, when the 'Forget me' button is pressed.
    public async Task<IActionResult> OnPost([FromQuery] string h)
    {
        if (h == "h")
        {
            Console.WriteLine("OnPostDeleteAuthor called.");

            // I want to make a break for 10 seconds here

            string authorName = User.Identity.Name; // Author.Name;

            var _Author = await _Author_repository.FindAuthorByName(authorName);

            await _Author_repository.ForgetMe(authorName);
        }
        return Page();
    }
}
