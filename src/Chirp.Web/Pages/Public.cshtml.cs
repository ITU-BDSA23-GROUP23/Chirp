using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Chirp.Web.Pages.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Humanizer;
using SQLitePCL;

namespace Chirp.Web.Pages;
[AllowAnonymous]
public class PublicModel : PageModel
{
    private readonly IAuthorRepository authorRepository;
    private readonly ICheepService _service;
    private readonly ILogger<PublicModel> _logger;
    public IEnumerable<CheepDTO>? Cheeps { get; set; }
    public PageNavModel PageNav;
    public int TotalPages { get; set; }

    public PublicModel(ICheepService service, ILogger<PublicModel> logger, IAuthorRepository authorRepository)
    {
        _service = service;
        _logger = logger;
        this.authorRepository = authorRepository;
        //Cheeps = service.GetCheeps(null);
    }

    public ActionResult OnGet([FromQuery] int page)
    {
        var _Cheeps = _service.GetCheeps(page);
        _Cheeps.Wait();
        Cheeps = _Cheeps.Result;

        var _TotalPages = _service.GetPageAmount();
        _TotalPages.Wait();
        TotalPages = _TotalPages.Result;
        PageNav = new PageNavModel(_service, page, TotalPages);

        return Page();
    }

    public async Task<ActionResult> OnPost([FromQuery] int page, [FromQuery] string handler) {
        Console.WriteLine("OnPost called!");
        var _Cheeps = _service.GetCheeps(page);
        _Cheeps.Wait();

        Cheeps = _Cheeps.Result;
        var _TotalPages = _service.GetPageAmount();
        _TotalPages.Wait();
        TotalPages = _TotalPages.Result;
        PageNav = new PageNavModel(_service, page, TotalPages);
/*
        if (handler == "follow")
        {
            Console.WriteLine("OnPostFollow Called in OnPost");
            if(User.Identity.Name != null)
            {
                string? AuthorName = Request.Form["Follow"];
                await FollowAuthor(User.Identity.Name, AuthorName);
            }
            else
            {
                string? AuthorName = Request.Form["Follow"];
                Console.WriteLine("AuthorName is: " + AuthorName);
                await FollowAuthor(AuthorName);
            }
        }
*/
        return Page();
    }
    public void OnPostFollow([FromQuery] int page)
    {
        Console.WriteLine("OnPostFollow Called");
        if(User.Identity.Name != null)
        {
            string? AuthorName = Request.Form["Follow"];
            FollowAuthor(AuthorName);
        }
        else
        {
            string? AuthorName = Request.Form["Follow"];
            Console.WriteLine("AuthorName is: " + AuthorName);
            FollowAuthor(AuthorName);
        }
        //return RedirectToPage("/");
    }
    // for testing purposes only
    public async Task FollowAuthor(string followerName)
    {
        Console.WriteLine("!!!This is a test, calling Followauthor with a temp user (Not logged in)");
        Console.WriteLine("FollowerName: " + followerName);
        var _tempFollowing = await authorRepository.FindAuthorByName("temp");
        Console.WriteLine("Now calling FollowAuthor");
        await FollowAuthor(followerName, _tempFollowing.Name);
    }


    public async Task FollowAuthor(string followerName, string followingName)
    {
        Console.WriteLine($"FollowAuthor called with followerName: {followerName}, followingName: {followingName}");

        var _follower = await authorRepository.FindAuthorByName(followerName);
        var _following = await authorRepository.FindAuthorByName(followingName);


        Console.WriteLine($"FollowAuthor is now using the following: {_follower} as follower, and {_following} as following");

        await authorRepository.FollowAuthor(_follower, _following);
    }

    public async Task UnfollowAuthor(string followerName, string followingName)
    {

        //Console.WriteLine($"UnfollowAuthor called with followerName: {followerName}, followingName: {followingName}");

        var _follower =  authorRepository.FindAuthorByName(followerName);
        _follower.Wait();
        var _following = authorRepository.FindAuthorByName(followingName);
        _following.Wait();

        Console.WriteLine($"UnfollowAuthor is now using the following: {_follower.Result} as follower, and {_following.Result} as following");

        await authorRepository.UnfollowAuthor(_follower.Result, _following.Result);
    }
}
