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

namespace Chirp.Web.Pages;
[AllowAnonymous]
public class PublicModel : PageModel
{

    private readonly ICheepService _service;
    private readonly ILogger<PublicModel> _logger;
    public IEnumerable<CheepDTO>? Cheeps { get; set; }
    public PageNavModel PageNav;

    public ChirpDBContext _dbContext;

    public int TotalPages { get; set; }

    public PublicModel(ICheepService service, ILogger<PublicModel> logger, ChirpDBContext dbContext)
    {
        _service = service;
        _logger = logger;
        _dbContext = dbContext;
        //Cheeps = service.GetCheeps(null);
    }

    public ActionResult OnGet([FromQuery] int page)
    {
        PageNav = new PageNavModel(_service, page);

        var _Cheeps = _service.GetCheeps(page);
        _Cheeps.Wait();
        Cheeps = _Cheeps.Result;

        var _TotalPages = _service.GetPageAmount();
        _TotalPages.Wait();
        TotalPages = _TotalPages.Result;

        return Page();
    }

    public async Task FollowAuthor(string followerName, string followingName)
    {
        Console.WriteLine($"FollowAuthor called with followerName: {followerName}, followingName: {followingName}");

        Author? follower = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Name == followerName);
        Author? following = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Name == followingName);

        if (follower != null && following != null)
        {
            follower.Following.Add(following);
            following.Followers.Add(follower);
            Console.WriteLine("Followed");
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            throw new NullReferenceException($"Author {followerName} or {followingName} does not exist.");
        }
    }

    public async Task UnfollowAuthor(string followerName, string followingName)
    {

        Console.WriteLine($"UnfollowAuthor called with followerName: {followerName}, followingName: {followingName}");

        Author? follower = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Name == followerName);
        Author? following = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Name == followingName);

        if (follower != null && following != null)
        {
            follower.Following.Remove(following);
            following.Followers.Remove(follower);
            Console.WriteLine("Unfollowed");
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            throw new NullReferenceException($"Author {followerName} or {followingName} does not exist.");
        }
    }
}