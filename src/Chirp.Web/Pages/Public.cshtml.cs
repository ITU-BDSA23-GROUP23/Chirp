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
        PageNav = new PageNavModel(_service, 1, TotalPages);
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

    public async Task OnPost([FromQuery] int page, [FromQuery] string f, [FromQuery] string uf)
    {
        Console.WriteLine("OnPost called!");
        var _Cheeps = await _service.GetCheeps(page);
        Cheeps = _Cheeps;

        var _TotalPages = await _service.GetPageAmount();
        TotalPages = _TotalPages;
        PageNav = new PageNavModel(_service, page, TotalPages);
        
        Console.WriteLine($"uf is {uf}");
        if (f != null)
        {
            string? AuthorName = Request.Form["Follow"];
            await FollowAuthor(f, AuthorName);
        } else if (uf != null)
        {
            string? AuthorName = Request.Form["Unfollow"];
            await UnfollowAuthor(uf, AuthorName);

        }
    }

    
    
    public async Task FollowAuthor(string followerName, string followingName)
    {
        Console.WriteLine($"FollowAuthor called with followerName: {followerName}, followingName: {followingName}");
        
        var _follower = await authorRepository.FindAuthorByName(followerName);
        if (_follower == null) 
        {
            authorRepository.CreateAuthor(new CreateAuthorDTO(followerName, ""));
            _follower = await authorRepository.FindAuthorByName(followerName);
        }

        var _following = await authorRepository.FindAuthorByName(followingName);

        await authorRepository.FollowAuthor(_following, _follower);
    }

    public async Task UnfollowAuthor(string followerName, string followingName)
    {

        Console.WriteLine($"UnfollowAuthor called with followerName: {followerName}, followingName: {followingName}");

        var _follower = await authorRepository.FindAuthorByName(followerName);
        if (_follower == null) 
        {
            Console.WriteLine("Follower is null");
            authorRepository.CreateAuthor(new CreateAuthorDTO(followerName, ""));
            _follower = await authorRepository.FindAuthorByName(followerName);
        }

        var _following = await authorRepository.FindAuthorByName(followingName);

        await authorRepository.UnfollowAuthor(_following, _follower);
    }

    public async Task<bool> IsFollowing(string self, string other)
    {
        Console.WriteLine("IsFollowing called");
        var _self = await authorRepository.FindAuthorByName(self);
        if (_self == null)
        {
            return false;
        }
        var _other = await authorRepository.FindAuthorByName(other);
        if (_other.Followers.Contains(_self.Id))
        {
            return true;
        }
        return false;
    }
}
