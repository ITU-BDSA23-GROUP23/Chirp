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

    private readonly ICheepService _service;
    private readonly ILogger<PublicModel> _logger;

    public PageInfoModel(ICheepService service, ILogger<PublicModel> logger, IAuthorRepository Author_repository, ICheepRepository Cheep_repository)
    {
        _service = service;
        _logger = logger;
        _Author_repository = Author_repository;
        _Cheep_repository = Cheep_repository;
    }

    // get author name and email
    public void OnGetAuthor(string author)
    {
        var _Author = _Author_repository.FindAuthorByName(author);
        _Author.Wait();
        Author = _Author.Result;
    }

    // list of cheeps
    public ActionResult OnGet(string author, [FromQuery] int page)
    {
        var _Cheeps = _service.GetCheepsFromAuthor(author, page);
        _Cheeps.Wait();
        Cheeps = _Cheeps.Result;
        return Page();
    }

    // list of followers for author
    public int FollowingCount(string author)
    {
        var _author = _Author_repository.FindAuthorByName(author);
        _author.Wait();
        Console.WriteLine($"Is author null? = {_author == null}");
        //Console.WriteLine(_author.Result.Following.ToArray()[0]);
        var _FollowingCount = _author.Result.Following.Count;
        Console.WriteLine(_FollowingCount + " is the result of _FollowingCount");
        return _FollowingCount;
    }
    public int FollowersCount(string author)
    {
        var _author = _Author_repository.FindAuthorByName(author);
        _author.Wait();
        var _FollowersCount = _author.Result.Followers.Count;
        Console.WriteLine(_FollowersCount + " is the result of _FollowersCount");
        return _FollowersCount;
    }

    //make async task that calls removeAuthor
    public async Task<IActionResult> OnPostDeleteAuthor(string author)
    {
        var _Author = _Author_repository.FindAuthorByName(author);
        _Author.Wait();
        var _Cheeps = _Cheep_repository.GetCheeps(1, 1, author);
        _Cheeps.Wait();
        var _Followers = _Author_repository.GetFollowers(author);
        _Followers.Wait();
        var _Following = _Author_repository.GetFollowing(author);
        _Following.Wait();
        var _CheepAmount = _Author_repository.GetCheepAmount(author);
        _CheepAmount.Wait();

        await _Author_repository.DeleteAuthor(author);
        Console.WriteLine($"Author {_Author.Result.Name} deleted.");
        await _Cheep_repository.RemoveCheeps(_Cheeps.Result);
        Console.WriteLine($"Cheeps deleted.");
        await _Author_repository.RemoveFollowers(_Followers.Result);
        Console.WriteLine($"Followers deleted.");
        await _Author_repository.RemoveFollowing(_Following.Result);
        Console.WriteLine($"Following deleted.");

        return RedirectToPage("/Index");
    }
}