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
    public void OnGetFollowers(string author)
    {
        var _Followers = _Author_repository.GetFollowers(author);
        _Followers.Wait();
        Followers = _Followers.Result;
    }

    // list of following for author
    public void OnGetFollowing(string author)
    {
        var _Following = _Author_repository.GetFollowing(author);
        _Following.Wait();
        Following = _Following.Result;
    }

    // number of cheeps for author
    public void OnGetCheepAmount(string author)
    {
        var _CheepAmount = _Author_repository.GetCheepAmount(author);
        _CheepAmount.Wait();
        CheepAmount = _CheepAmount.Result;
    }

    public int FollowingCount(string authorName)
    {
        if (authorName == null)
        {
            return 0;
        }
        else
        {
            var _author = _Author_repository.FindAuthorByName(authorName);
            _author.Wait();
            var _FollowingCount = _author.Result.Following.Count;
            Console.WriteLine(_FollowingCount + " is the result of _FollowingCount");
            return _FollowingCount;
        }
    }

    public int FollowersCount(string authorName)
    {
        if (authorName == null)
        {
            return 0;
        }
        else
        {
            var _author = _Author_repository.FindAuthorByName(authorName);
            _author.Wait();
            var _FollowersCount = _author.Result.Followers.Count;
            Console.WriteLine(_FollowersCount + " is the result of _FollowersCount");
            return _FollowersCount;
        }
    }


    //****ADD FORGET ME FEATURE****

}