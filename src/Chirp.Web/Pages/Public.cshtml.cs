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

    public async Task FollowAuthor(string followerName, string followingName)
    {
        Console.WriteLine($"FollowAuthor called with followerName: {followerName}, followingName: {followingName}");

        var follower = await authorRepository.FindAuthorByName(followerName);
        var following = await authorRepository.FindAuthorByName(followingName);

        await authorRepository.FollowAuthor(follower, following);
    }

    public async Task UnfollowAuthor(string followerName, string followingName)
    {

        Console.WriteLine($"UnfollowAuthor called with followerName: {followerName}, followingName: {followingName}");

        var follower = await authorRepository.FindAuthorByName(followerName);
        var following = await authorRepository.FindAuthorByName(followingName);

        await authorRepository.UnfollowAuthor(follower, following);
    }
}